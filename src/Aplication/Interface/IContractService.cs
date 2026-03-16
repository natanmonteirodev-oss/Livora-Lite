using Livora_Lite.Application.DTO;

namespace Livora_Lite.Application.Interface
{
    public interface IContractService
    {
        Task<IEnumerable<ContractDTO>> GetAllContractsAsync();
        Task<ContractDTO?> GetByIdAsync(int id);
        Task<ContractDTO> CreateAsync(CreateContractRequestDTO request);
        Task UpdateAsync(UpdateContractRequestDTO request);
        Task DeleteAsync(int id);
        Task<IEnumerable<ContractDTO>> GetActiveContractsAsync();
        Task<IEnumerable<ContractDTO>> GetContractsByPropertyAsync(int propertyId);
        Task<IEnumerable<ContractDTO>> GetContractsByTenantAsync(int tenantId);
        Task<bool> HasActiveContractForPropertyAsync(int propertyId);
    }
}