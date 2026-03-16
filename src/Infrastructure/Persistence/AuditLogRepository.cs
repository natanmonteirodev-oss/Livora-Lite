using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure.Persistence
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly LivoraDbContext _context;

        public AuditLogRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuditLog>> GetAllAuditLogsAsync()
        {
            return await _context.AuditLogs
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<AuditLog> GetAuditLogByIdAsync(int id)
        {
            return await _context.AuditLogs.FindAsync(id);
        }

        public async Task AddAuditLogAsync(AuditLog auditLog)
        {
            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuditLogAsync(AuditLog auditLog)
        {
            _context.AuditLogs.Update(auditLog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuditLogAsync(int id)
        {
            var auditLog = await _context.AuditLogs.FindAsync(id);
            if (auditLog != null)
            {
                _context.AuditLogs.Remove(auditLog);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByEntityAsync(string entity, string entityId)
        {
            return await _context.AuditLogs
                .Where(a => a.Entity == entity && a.EntityId == entityId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsByUserAsync(string userId)
        {
            return await _context.AuditLogs
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }
    }
}