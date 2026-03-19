using Livora_Lite.Application.Interface;
using Livora_Lite.Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Livora_Lite.Controllers
{
    [Authorize]
    public class AuditLogsController : Controller
    {
        private readonly IAuditService _auditService;

        public AuditLogsController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        // GET: AuditLogs
        public async Task<IActionResult> Index(string entity, string userName, DateTime? startDate, DateTime? endDate)
        {
            var auditLogs = await _auditService.GetAllAuditLogsAsync();
            
            // Aplicar filtros
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
                // Incluir o final do dia
                var endOfDay = endDate.Value.Date.AddDays(1).AddTicks(-1);
                auditLogs = auditLogs.Where(a => a.Date <= endOfDay);
            }
            
            return View(auditLogs);
        }

        // GET: AuditLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditLog = await _auditService.GetAuditLogByIdAsync(id.Value);
            if (auditLog == null)
            {
                return NotFound();
            }

            return View(auditLog);
        }

        // GET: AuditLogs/ByEntity?entity=Property&entityId=1
        public async Task<IActionResult> ByEntity(string entity, string entityId)
        {
            if (string.IsNullOrEmpty(entity) || string.IsNullOrEmpty(entityId))
            {
                return BadRequest("Entity and EntityId are required.");
            }

            var auditLogs = await _auditService.GetAuditLogsByEntityAsync(entity, entityId);
            ViewData["Entity"] = entity;
            ViewData["EntityId"] = entityId;
            return View(auditLogs);
        }

        // GET: AuditLogs/ByUser?userId=123
        public async Task<IActionResult> ByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is required.");
            }

            var auditLogs = await _auditService.GetAuditLogsByUserAsync(userId);
            ViewData["UserId"] = userId;
            return View(auditLogs);
        }
        // GET: AuditLogs/Export
        public async Task<IActionResult> Export(string entity, string userName, DateTime? startDate, DateTime? endDate)
        {
            var auditLogs = await _auditService.GetAllAuditLogsAsync();
            
            // Aplicar os mesmos filtros
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
            
            // Gerar CSV
            var csv = new StringBuilder();
            csv.AppendLine("ID,Usuário,Ação,Entidade,EntidadeID,Data,Alterações");
            
            foreach (var log in auditLogs)
            {
                csv.AppendLine($"{log.Id},{log.UserName},{log.Action},{log.Entity},{log.EntityId},{log.Date:yyyy-MM-dd HH:mm:ss},\"{log.Changes?.Replace("\"", "\"\"")}\"");
            }
            
            var fileName = $"audit_logs_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", fileName);
        }
    }
}