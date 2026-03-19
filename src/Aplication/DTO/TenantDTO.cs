namespace Livora_Lite.Application.DTO
{
    public class TenantDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CurrentAddress { get; set; } = string.Empty;
        public TenantStatusDTO? TenantStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Propriedade calculada para exibição
        public string Name => $"{FirstName} {LastName}".Trim();
    }
}