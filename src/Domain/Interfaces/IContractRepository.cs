using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IContractRepository
    {
        Task<Contract?> GetByIdAsync(int id);
        Task<IEnumerable<Contract>> GetAllAsync();
        Task<Contract> CreateAsync(Contract contract);
        Task<Contract> UpdateAsync(Contract contract);
        Task DeleteAsync(int id);
        Task<IEnumerable<Contract>> GetActiveContractsAsync();
        Task<IEnumerable<Contract>> GetContractsByPropertyAsync(int propertyId);
        Task<IEnumerable<Contract>> GetContractsByTenantAsync(int tenantId);
        Task<bool> HasActiveContractForPropertyAsync(int propertyId);
    }
}