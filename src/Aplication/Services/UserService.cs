#nullable disable

using AutoMapper;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Livora_Lite.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> CreateUserAsync(CreateUserRequestDTO request)
        {
            var passwordHash = HashPassword(request.Password);
            
            var user = _mapper.Map<User>(request);
            user.PasswordHash = passwordHash;
            user.IsActive = true;
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.CreateAsync(user);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> UpdateUserAsync(UpdateUserRequestDTO request)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                throw new Exception("Usuário não encontrado");

            _mapper.Map(request, user);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            await _userRepository.DeleteAsync(user);
            return true;
        }

        public Task<bool> ValidatePasswordAsync(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return Task.FromResult(hashOfInput == hash);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var encryptedPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(encryptedPassword);
            }
        }

    }
}
