using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.Interface;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Relatórios - Gerencia geração de relatórios financeiros e operacionais
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Owner")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Gera relatório financeiro com resumo de receitas e despesas
        /// </summary>
        /// <returns>Dados do relatório financeiro</returns>
        [HttpGet("financial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetFinancialReport()
        {
            try
            {
                var report = await _reportService.GetFinancialReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao gerar relatório financeiro: {ex.Message}" });
            }
        }

        /// <summary>
        /// Gera relatório de performance das propriedades
        /// </summary>
        /// <returns>Dados do relatório de performance</returns>
        [HttpGet("property-performance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPropertyPerformanceReport()
        {
            try
            {
                var report = await _reportService.GetPropertyPerformanceReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao gerar relatório de propriedades: {ex.Message}" });
            }
        }

        /// <summary>
        /// Gera análise de contratos ativos e vencidos
        /// </summary>
        /// <returns>Dados da análise de contratos</returns>
        [HttpGet("contract-analysis")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetContractAnalysisReport()
        {
            try
            {
                var report = await _reportService.GetContractAnalysisReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao gerar relatório de contratos: {ex.Message}" });
            }
        }

        /// <summary>
        /// Gera relatório de solicitações de manutenção
        /// </summary>
        /// <returns>Dados do relatório de manutenção</returns>
        [HttpGet("maintenance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetMaintenanceReport()
        {
            try
            {
                var report = await _reportService.GetMaintenanceReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Erro ao gerar relatório de manutenção: {ex.Message}" });
            }
        }
    }
}
