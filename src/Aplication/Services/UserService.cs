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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : MapToDTO(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDTO).ToList();
        }

        public async Task<UserDTO> CreateUserAsync(CreateUserRequestDTO request)
        {
            var passwordHash = HashPassword(request.Password);
            
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = request.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            return MapToDTO(user);
        }

        public async Task<UserDTO> UpdateUserAsync(UpdateUserRequestDTO request)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                throw new Exception("Usuário não encontrado");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Role = request.Role;
            user.IsActive = request.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            return MapToDTO(user);
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

        private UserDTO MapToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
