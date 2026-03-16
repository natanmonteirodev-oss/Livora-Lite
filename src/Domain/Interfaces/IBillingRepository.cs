using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IBillingRepository
    {
        Task<Billing?> GetByIdAsync(int id);
        Task<IEnumerable<Billing>> GetAllAsync();
        Task<Billing> CreateAsync(Billing billing);
        Task<Billing> UpdateAsync(Billing billing);
        Task DeleteAsync(int id);
        Task<IEnumerable<Billing>> GetBillingsByContractAsync(int contractId);
        Task<IEnumerable<Billing>> GetBillingsByPeriodAsync(string period);
        Task<bool> HasBillingForContractAndPeriodAsync(int contractId, string period);
    }
}