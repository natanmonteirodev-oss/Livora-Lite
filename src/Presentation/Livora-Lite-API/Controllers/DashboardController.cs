using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.Interface;
using System.Security.Claims;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Dashboard - Fornece dados de dashboard personalizados por função do usuário
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Obtém dashboard para administrador com visão geral do sistema
        /// </summary>
        /// <returns>Dados do dashboard admin</returns>
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAdminDashboard()
        {
            try
            {
                var dashboard = await _dashboardService.GetAdminDashboardAsync();
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao carregar dashboard: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtém dashboard para proprietário com visão de suas propriedades e receitas
        /// </summary>
        /// <returns>Dados do dashboard do proprietário</returns>
        [HttpGet("owner")]
        [Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetOwnerDashboard()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return BadRequest(new { message = "Erro ao identificar usuário" });
                }

                var dashboard = await _dashboardService.GetOwnerDashboardAsync(userId);
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao carregar dashboard: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtém dashboard para inquilino com informações de contrato e pagamentos
        /// </summary>
        /// <returns>Dados do dashboard do inquilino</returns>
        [HttpGet("tenant")]
        [Authorize(Roles = "Tenant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetTenantDashboard()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return BadRequest(new { message = "Erro ao identificar usuário" });
                }

                var dashboard = await _dashboardService.GetTenantDashboardAsync(userId);
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao carregar dashboard: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtém dashboard personalizado do usuário atual baseado em sua função
        /// </summary>
        /// <returns>Dados do dashboard do usuário atual</returns>
        [HttpGet("current")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUserDashboard()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return BadRequest(new { message = "Erro ao identificar usuário" });
                }

                object dashboardData;

                if (User.IsInRole("Admin"))
                {
                    dashboardData = await _dashboardService.GetAdminDashboardAsync();
                }
                else if (User.IsInRole("Owner"))
                {
                    dashboardData = await _dashboardService.GetOwnerDashboardAsync(userId);
                }
                else
                {
                    dashboardData = await _dashboardService.GetTenantDashboardAsync(userId);
                }

                return Ok(dashboardData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao carregar dashboard: {ex.Message}" });
            }
        }
    }
}
