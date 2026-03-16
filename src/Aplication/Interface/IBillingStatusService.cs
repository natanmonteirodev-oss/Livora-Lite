using Livora_Lite.Application.DTO;

namespace Livora_Lite.Application.Interface
{
    public interface IBillingStatusService
    {
        Task<IEnumerable<BillingStatusDTO>> GetAllBillingStatusesAsync();
        Task<BillingStatusDTO?> GetBillingStatusByIdAsync(int id);
    }
}