using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Livora_Lite.Infrastructure.Persistence
{
    public static class SeedData
    {
        public static async Task SeedAdminUserAsync(IUserRepository userRepository)
        {
            // Verificar se o admin já existe
            var adminExists = await userRepository.ExistsAsync("admin@livora.com");
            
            if (adminExists)
                return;

            // Hash da senha @d1mn
            var passwordHash = HashPassword("@d1mn");

            var adminUser = new User
            {
                FirstName = "Admin",
                LastName = "Livora",
                Email = "admin@livora.com",
                PasswordHash = passwordHash,
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await userRepository.CreateAsync(adminUser);
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var encryptedPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(encryptedPassword);
            }
        }
    }
}
