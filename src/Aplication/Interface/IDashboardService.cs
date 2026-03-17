using Livora_Lite.Application.DTO;

namespace Livora_Lite.Application.Interface
{
    public interface IDashboardService
    {
        Task<AdminDashboardDTO> GetAdminDashboardAsync();
        Task<OwnerDashboardDTO> GetOwnerDashboardAsync(int userId);
        Task<TenantDashboardDTO> GetTenantDashboardAsync(int userId);
    }
}
