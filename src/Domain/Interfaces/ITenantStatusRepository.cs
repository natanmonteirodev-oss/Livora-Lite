using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface ITenantStatusRepository
    {
        Task<TenantStatus?> GetByIdAsync(int id);
        Task<IEnumerable<TenantStatus>> GetAllAsync();
        Task<TenantStatus> CreateAsync(TenantStatus tenantStatus);
        Task<TenantStatus> UpdateAsync(TenantStatus tenantStatus);
        Task<bool> DeleteAsync(int id);
    }
}