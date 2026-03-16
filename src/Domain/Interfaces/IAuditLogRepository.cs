using System.Collections.Generic;
using System.Threading.Tasks;
using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IAuditLogRepository
    {
        Task<IEnumerable<AuditLog>> GetAllAuditLogsAsync();
        Task<AuditLog> GetAuditLogByIdAsync(int id);
        Task AddAuditLogAsync(AuditLog auditLog);
        Task UpdateAuditLogAsync(AuditLog auditLog);
        Task DeleteAuditLogAsync(int id);
        Task<IEnumerable<AuditLog>> GetAuditLogsByEntityAsync(string entity, string entityId);
        Task<IEnumerable<AuditLog>> GetAuditLogsByUserAsync(string userId);
    }
}