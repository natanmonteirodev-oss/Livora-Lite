using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IContractStatusRepository
    {
        Task<ContractStatus?> GetByIdAsync(int id);
        Task<IEnumerable<ContractStatus>> GetAllAsync();
        Task<ContractStatus> CreateAsync(ContractStatus contractStatus);
        Task<ContractStatus> UpdateAsync(ContractStatus contractStatus);
        Task DeleteAsync(int id);
    }
}