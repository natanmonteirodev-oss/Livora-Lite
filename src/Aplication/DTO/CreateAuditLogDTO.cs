using System;

namespace Livora_Lite.Application.DTO
{
    public class CreateAuditLogDTO
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Action { get; set; }
        public string? Entity { get; set; }
        public string? EntityId { get; set; }
        public string? Changes { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
    }
}