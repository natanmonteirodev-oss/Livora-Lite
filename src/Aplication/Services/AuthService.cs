using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Livora_Lite.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };

                return new AuthResponseDTO
                {
                    Success = true,
                    Message = "Login realizado com sucesso.",
                    User = userDTO
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

                var userDTO = new UserDTO
                {
                    Id = createdUser.Id,
                    FirstName = createdUser.FirstName,
                    LastName = createdUser.LastName,
                    Email = createdUser.Email
                };

                return new AuthResponseDTO
                {
                    Success = true,
                    Message = "Usuário registrado com sucesso.",
                    User = userDTO
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
            if (user == null)
                return null;

            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
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

        private static bool ValidatePassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hash);
        }
    }
}
