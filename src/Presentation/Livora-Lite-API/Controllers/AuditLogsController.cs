using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Livora_Lite.Application.Interface;
using System.Text;

namespace Livora_Lite_API.Controllers
{
    /// <summary>
    /// Controlador de Logs de Auditoria - Gerencia consulta, filtro e exportação de registros de auditoria
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditLogsController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        /// <summary>
        /// Lista todos os logs de auditoria com filtros opcionais
        /// </summary>
        /// <param name="entity">Filtro por tipo de entidade (opcional)</param>
        /// <param name="userName">Filtro por nome de usuário (opcional)</param>
        /// <param name="startDate">Filtro por data inicial (opcional)</param>
        /// <param name="endDate">Filtro por data final (opcional)</param>
        /// <returns>Lista de logs de auditoria</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll([FromQuery] string? entity, string? userName, DateTime? startDate, DateTime? endDate)
        {
            var auditLogs = await _auditService.GetAllAuditLogsAsync();

            // Apply filters
            if (!string.IsNullOrEmpty(entity))
            {
                auditLogs = auditLogs.Where(a => a.Entity == entity);
            }

            if (!string.IsNullOrEmpty(userName))
            {
                auditLogs = auditLogs.Where(a => a.UserName != null && a.UserName.Contains(userName, StringComparison.OrdinalIgnoreCase));
            }

            if (startDate.HasValue)
            {
                auditLogs = auditLogs.Where(a => a.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                var endOfDay = endDate.Value.Date.AddDays(1).AddTicks(-1);
                auditLogs = auditLogs.Where(a => a.Date <= endOfDay);
            }

            return Ok(auditLogs);
        }

        /// <summary>
        /// Obtém um log de auditoria específico pelo ID
        /// </summary>
        /// <param name="id">ID do log de auditoria</param>
        /// <returns>Detalhes do log de auditoria</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var auditLog = await _auditService.GetAuditLogByIdAsync(id);
            if (auditLog == null)
                return NotFound();

            return Ok(auditLog);
        }

        /// <summary>
        /// Obtém logs de auditoria por tipo e ID de entidade
        /// </summary>
        /// <param name="entity">Tipo de entidade (obrigatório)</param>
        /// <param name="entityId">ID da entidade (obrigatório)</param>
        /// <returns>Lista de logs relacionados à entidade</returns>
        [HttpGet("by-entity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByEntity([FromQuery] string entity, string entityId)
        {
            if (string.IsNullOrEmpty(entity) || string.IsNullOrEmpty(entityId))
            {
                return BadRequest(new { message = "Entity and EntityId are required." });
            }

            var auditLogs = await _auditService.GetAuditLogsByEntityAsync(entity, entityId);
            return Ok(auditLogs);
        }

        /// <summary>
        /// Obtém logs de auditoria por usuário
        /// </summary>
        /// <param name="userId">ID do usuário (obrigatório)</param>
        /// <returns>Lista de logs do usuário</returns>
        [HttpGet("by-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByUser([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "UserId is required." });
            }

            var auditLogs = await _auditService.GetAuditLogsByUserAsync(userId);
            return Ok(auditLogs);
        }

        /// <summary>
        /// Exporta logs de auditoria em formato CSV
        /// </summary>
        /// <param name="entity">Filtro por tipo de entidade (opcional)</param>
        /// <param name="userName">Filtro por nome de usuário (opcional)</param>
        /// <param name="startDate">Filtro por data inicial (opcional)</param>
        /// <param name="endDate">Filtro por data final (opcional)</param>
        /// <returns>Arquivo CSV com os logs de auditoria</returns>
        [HttpGet("export")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Export([FromQuery] string? entity, string? userName, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var auditLogs = await _auditService.GetAllAuditLogsAsync();

                // Apply the same filters
                if (!string.IsNullOrEmpty(entity))
                {
                    auditLogs = auditLogs.Where(a => a.Entity == entity);
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    auditLogs = auditLogs.Where(a => a.UserName != null && a.UserName.Contains(userName, StringComparison.OrdinalIgnoreCase));
                }

                if (startDate.HasValue)
                {
                    auditLogs = auditLogs.Where(a => a.Date >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    var endOfDay = endDate.Value.Date.AddDays(1).AddTicks(-1);
                    auditLogs = auditLogs.Where(a => a.Date <= endOfDay);
                }

                // Convert to CSV
                var csv = ConvertToCsv(auditLogs.ToList());
                var bytes = Encoding.UTF8.GetBytes(csv);

                return File(bytes, "text/csv", "audit_logs.csv");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao exportar logs: " + ex.Message });
            }
        }

        private string ConvertToCsv<T>(List<T> list)
        {
            var csv = new StringBuilder();
            
            if (list.Count == 0)
                return csv.ToString();

            // Get properties
            var properties = typeof(T).GetProperties();
            
            // Write header
            csv.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Write data
            foreach (var item in list)
            {
                var values = properties.Select(p => p.GetValue(item, null)?.ToString()?.Replace(",", ";") ?? "");
                csv.AppendLine(string.Join(",", values));
            }

            return csv.ToString();
        }
    }
}
