using AutoMapper;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Livora_Lite.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IAppSettingsRepository _appSettingsRepository;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IAppSettingsRepository appSettingsRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _appSettingsRepository = appSettingsRepository;
            _mapper = mapper;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "Email e senha são obrigatórios."
                    };
                }

                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user == null || !user.IsActive)
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "Usuário não encontrado ou inativo."
                    };
                }

                if (!ValidatePassword(request.Password, user.PasswordHash))
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "Senha incorreta."
                    };
                }

                var token = await GenerateJwtToken(user);

                var userDTO = _mapper.Map<UserDTO>(user);

                return new AuthResponseDTO
                {
                    Success = true,
                    Message = "Login realizado com sucesso.",
                    User = userDTO,
                    Token = token
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = $"Erro ao realizar login: {ex.Message}"
                };
            }
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO request)
        {
            try
            {
                // Validações
                if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "Nome e sobrenome são obrigatórios."
                    };
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "Email é obrigatório."
                    };
                }

                if (request.Password != request.ConfirmPassword)
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "As senhas não coincidem."
                    };
                }

                if (request.Password.Length < 6)
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "A senha deve ter no mínimo 6 caracteres."
                    };
                }

                // Verificar se usuário já existe
                if (await _userRepository.ExistsAsync(request.Email))
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "Este email já está registrado."
                    };
                }

                // Criar novo usuário
                var user = new User
                {
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    Email = request.Email.Trim().ToLower(),
                    PasswordHash = HashPassword(request.Password),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdUser = await _userRepository.CreateAsync(user);

                var token = await GenerateJwtToken(createdUser);

                var userDTO = _mapper.Map<UserDTO>(createdUser);

                return new AuthResponseDTO
                {
                    Success = true,
                    Message = "Usuário registrado com sucesso.",
                    User = userDTO,
                    Token = token
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = $"Erro ao registrar usuário: {ex.Message}"
                };
            }
        }

        public async Task<UserDTO?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user == null ? null : _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> ValidatePasswordAsync(string password, string hash)
        {
            return await Task.FromResult(ValidatePassword(password, hash));
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured");
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is not configured");
            var jwtAudience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is not configured");

            var timeoutSetting = await _appSettingsRepository.GetByKeyAsync("SessionTimeoutMinutes");
            int timeoutMinutes = 5; // default
            if (timeoutSetting != null && int.TryParse(timeoutSetting.Value, out int parsed))
            {
                timeoutMinutes = parsed;
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(timeoutMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static bool ValidatePassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hash);
        }
    }
}
