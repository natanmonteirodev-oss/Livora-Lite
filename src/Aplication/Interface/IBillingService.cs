using Livora_Lite.Application.DTO;

namespace Livora_Lite.Application.Interface
{
    public interface IBillingService
    {
        Task<IEnumerable<BillingDTO>> GetAllBillingsAsync();
        Task<BillingDTO?> GetBillingByIdAsync(int id);
        Task<BillingDTO> CreateBillingAsync(CreateBillingRequestDTO request);
        Task UpdateBillingAsync(UpdateBillingRequestDTO request);
        Task DeleteBillingAsync(int id);
        Task<IEnumerable<BillingDTO>> GetBillingsByContractAsync(int contractId);
        Task<IEnumerable<BillingDTO>> GetBillingsByPeriodAsync(string period);
        Task<bool> HasBillingForContractAndPeriodAsync(int contractId, string period);
    }
}