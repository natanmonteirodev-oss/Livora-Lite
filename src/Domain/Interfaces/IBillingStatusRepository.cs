using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IBillingStatusRepository
    {
        Task<BillingStatus?> GetByIdAsync(int id);
        Task<IEnumerable<BillingStatus>> GetAllAsync();
        Task<BillingStatus> CreateAsync(BillingStatus billingStatus);
        Task<BillingStatus> UpdateAsync(BillingStatus billingStatus);
        Task DeleteAsync(int id);
    }
}