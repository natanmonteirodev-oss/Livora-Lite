using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface ITenantRepository
    {
        Task<Tenant?> GetByIdAsync(int id);
        Task<IEnumerable<Tenant>> GetAllAsync();
        Task<Tenant?> GetByUserIdAsync(int userId);  // ← Novo método
        Task<Tenant> CreateAsync(Tenant tenant);
        Task<Tenant> UpdateAsync(Tenant tenant);
        Task<bool> DeleteAsync(int id);
    }
}