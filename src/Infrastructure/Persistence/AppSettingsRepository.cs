using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure
{
    public class AppSettingsRepository : IAppSettingsRepository
    {
        private readonly LivoraDbContext _context;

        public AppSettingsRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<AppSettings?> GetByKeyAsync(string key)
        {
            return await _context.AppSettings.FirstOrDefaultAsync(s => s.Key == key);
        }

        public async Task<AppSettings> CreateOrUpdateAsync(AppSettings setting)
        {
            var existing = await GetByKeyAsync(setting.Key);
            if (existing != null)
            {
                existing.Value = setting.Value;
                existing.UpdatedAt = DateTime.UtcNow;
                _context.AppSettings.Update(existing);
                await _context.SaveChangesAsync();
                return existing;
            }
            else
            {
                setting.CreatedAt = DateTime.UtcNow;
                _context.AppSettings.Add(setting);
                await _context.SaveChangesAsync();
                return setting;
            }
        }
    }
}