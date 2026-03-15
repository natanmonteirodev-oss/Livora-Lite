using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IAppSettingsRepository
    {
        Task<AppSettings?> GetByKeyAsync(string key);
        Task<AppSettings> CreateOrUpdateAsync(AppSettings setting);
    }
}