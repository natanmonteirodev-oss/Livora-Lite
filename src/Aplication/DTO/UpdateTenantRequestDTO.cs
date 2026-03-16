namespace Livora_Lite.Application.DTO
{
    public class UpdateTenantRequestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CurrentAddress { get; set; } = string.Empty;
        public int TenantStatusId { get; set; }
    }
}