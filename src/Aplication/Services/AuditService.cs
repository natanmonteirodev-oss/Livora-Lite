using AutoMapper;
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
        private readonly IMapper _mapper;

        public AuditService(IAuditLogRepository auditLogRepository, IMapper mapper)
        {
            _auditLogRepository = auditLogRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuditLogDTO>> GetAllAuditLogsAsync()
        {
            var auditLogs = await _auditLogRepository.GetAllAuditLogsAsync();
            return _mapper.Map<IEnumerable<AuditLogDTO>>(auditLogs);
        }

        public async Task<AuditLogDTO?> GetAuditLogByIdAsync(int id)
        {
            var auditLog = await _auditLogRepository.GetAuditLogByIdAsync(id);
            return auditLog != null ? _mapper.Map<AuditLogDTO>(auditLog) : null;
        }

        public async Task CreateAuditLogAsync(CreateAuditLogDTO auditLogDto)
        {
            var auditLog = _mapper.Map<AuditLog>(auditLogDto);
            auditLog.Date = DateTime.UtcNow;
            await _auditLogRepository.AddAuditLogAsync(auditLog);
        }

        public async Task UpdateAuditLogAsync(AuditLogDTO auditLogDto)
        {
            var auditLog = await _auditLogRepository.GetAuditLogByIdAsync(auditLogDto.Id);
            if (auditLog != null)
            {
                _mapper.Map(auditLogDto, auditLog);
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
            return _mapper.Map<IEnumerable<AuditLogDTO>>(auditLogs);
        }

        public async Task<IEnumerable<AuditLogDTO>> GetAuditLogsByUserAsync(string userId)
        {
            var auditLogs = await _auditLogRepository.GetAuditLogsByUserAsync(userId);
            return _mapper.Map<IEnumerable<AuditLogDTO>>(auditLogs);
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

    }
}