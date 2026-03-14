using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(User user);
        Task<bool> ExistsAsync(string email);
    }
}
