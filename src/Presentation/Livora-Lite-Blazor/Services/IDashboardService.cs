using Livora_Lite.Application.DTO;

namespace Livora_Lite_Blazor.Services
{
    /// <summary>
    /// Interface para serviço de dashboard
    /// Define os métodos disponíveis para obter dados do dashboard
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Obtém o dashboard para Admin
        /// </summary>
        /// <returns>Dados do dashboard administrativo</returns>
        Task<AdminDashboardDTO> GetAdminDashboardAsync();

        /// <summary>
        /// Obtém o dashboard para Owner (Proprietário)
        /// </summary>
        /// <param name="userId">ID do usuário proprietário</param>
        /// <returns>Dados do dashboard do proprietário</returns>
        Task<OwnerDashboardDTO> GetOwnerDashboardAsync(int userId);

        /// <summary>
        /// Obtém o dashboard para Tenant (Inquilino)
        /// </summary>
        /// <param name="userId">ID do usuário inquilino</param>
        /// <returns>Dados do dashboard do inquilino</returns>
        Task<TenantDashboardDTO> GetTenantDashboardAsync(int userId);
    }
}
