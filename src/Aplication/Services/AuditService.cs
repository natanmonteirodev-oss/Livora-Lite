using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite.Application.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<IEnumerable<AuditLogDTO>> GetAllAuditLogsAsync()
        {
            var auditLogs = await _auditLogRepository.GetAllAuditLogsAsync();
            return auditLogs.Select(MapToDTO);
        }

        public async Task<AuditLogDTO> GetAuditLogByIdAsync(int id)
        {
            var auditLog = await _auditLogRepository.GetAuditLogByIdAsync(id);
            return auditLog != null ? MapToDTO(auditLog) : null;
        }

        public async Task CreateAuditLogAsync(CreateAuditLogDTO auditLogDto)
        {
            var auditLog = new AuditLog
            {
                UserId = auditLogDto.UserId,
                UserName = auditLogDto.UserName,
                Action = auditLogDto.Action,
                Entity = auditLogDto.Entity,
                EntityId = auditLogDto.EntityId,
                Date = DateTime.UtcNow,
                Changes = auditLogDto.Changes,
                OldValues = auditLogDto.OldValues,
                NewValues = auditLogDto.NewValues
            };

            await _auditLogRepository.AddAuditLogAsync(auditLog);
        }

        public async Task UpdateAuditLogAsync(AuditLogDTO auditLogDto)
        {
            var auditLog = await _auditLogRepository.GetAuditLogByIdAsync(auditLogDto.Id);
            if (auditLog != null)
            {
                auditLog.UserId = auditLogDto.UserId;
                auditLog.UserName = auditLogDto.UserName;
                auditLog.Action = auditLogDto.Action;
                auditLog.Entity = auditLogDto.Entity;
                auditLog.EntityId = auditLogDto.EntityId;
                auditLog.Date = auditLogDto.Date;
                auditLog.Changes = auditLogDto.Changes;
                auditLog.OldValues = auditLogDto.OldValues;
                auditLog.NewValues = auditLogDto.NewValues;

                await _auditLogRepository.UpdateAuditLogAsync(auditLog);
            }
        }

        public async Task DeleteAuditLogAsync(int id)
        {
            await _auditLogRepository.DeleteAuditLogAsync(id);
        }

        public async Task<IEnumerable<AuditLogDTO>> GetAuditLogsByEntityAsync(string entity, string entityId)
        {
            var auditLogs = await _auditLogRepository.GetAuditLogsByEntityAsync(entity, entityId);
            return auditLogs.Select(MapToDTO);
        }

        public async Task<IEnumerable<AuditLogDTO>> GetAuditLogsByUserAsync(string userId)
        {
            var auditLogs = await _auditLogRepository.GetAuditLogsByUserAsync(userId);
            return auditLogs.Select(MapToDTO);
        }

        public async Task LogActionAsync(string userId, string userName, string action, string entity, string entityId, string changes, string? oldValues = null, string? newValues = null)
        {
            var auditLogDto = new CreateAuditLogDTO
            {
                UserId = userId,
                UserName = userName,
                Action = action,
                Entity = entity,
                EntityId = entityId,
                Changes = changes,
                OldValues = oldValues,
                NewValues = newValues
            };

            await CreateAuditLogAsync(auditLogDto);
        }

        private AuditLogDTO MapToDTO(AuditLog auditLog)
        {
            return new AuditLogDTO
            {
                Id = auditLog.Id,
                UserId = auditLog.UserId,
                UserName = auditLog.UserName,
                Action = auditLog.Action,
                Entity = auditLog.Entity,
                EntityId = auditLog.EntityId,
                Date = auditLog.Date,
                Changes = auditLog.Changes,
                OldValues = auditLog.OldValues,
                NewValues = auditLog.NewValues
            };
        }
    }
}