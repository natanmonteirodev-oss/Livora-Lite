using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly LivoraDbContext _context;

        public UserRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
