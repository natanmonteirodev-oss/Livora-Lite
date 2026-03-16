namespace Livora_Lite.Domain.Entities
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty; // CPF or CNPJ
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CurrentAddress { get; set; } = string.Empty; // Current address as text
        public int TenantStatusId { get; set; }
        public TenantStatus? TenantStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}