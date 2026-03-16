using System.Collections.Generic;
using System.Threading.Tasks;
using Livora_Lite.Application.DTO;

namespace Livora_Lite.Application.Interface
{
    public interface IAuditService
    {
        Task<IEnumerable<AuditLogDTO>> GetAllAuditLogsAsync();
        Task<AuditLogDTO?> GetAuditLogByIdAsync(int id);
        Task CreateAuditLogAsync(CreateAuditLogDTO auditLogDto);
        Task UpdateAuditLogAsync(AuditLogDTO auditLogDto);
        Task DeleteAuditLogAsync(int id);
        Task<IEnumerable<AuditLogDTO>> GetAuditLogsByEntityAsync(string entity, string entityId);
        Task<IEnumerable<AuditLogDTO>> GetAuditLogsByUserAsync(string userId);
        Task LogActionAsync(string userId, string userName, string action, string entity, string entityId, string changes, string? oldValues = null, string? newValues = null);
    }
}