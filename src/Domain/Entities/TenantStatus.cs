namespace Livora_Lite.Domain.Entities
{
    public class TenantStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Ativo, Inativo, Bloqueado, etc.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}