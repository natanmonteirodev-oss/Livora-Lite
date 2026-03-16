using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Application.DTO
{
    public class MaintenanceRequestDTO
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int? ContractId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public MaintenancePriority Priority { get; set; }
        public MaintenanceStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public PropertyDTO? Property { get; set; }
        public ContractDTO? Contract { get; set; }
    }
}