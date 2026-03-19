using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Entities
{
    public class MaintenanceRequest
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int? ContractId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public MaintenancePriority Priority { get; set; }
        public MaintenanceStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public Property? Property { get; set; }
        public Contract? Contract { get; set; }
    }

    public enum MaintenancePriority
    {
        Baixa = 1,
        Media = 2,
        Alta = 3,
        Urgente = 4
    }

    public enum MaintenanceStatus
    {
        Aberta = 1,
        EmAndamento = 2,
        AguardandoOrcamento = 3,
        Concluida = 4
    }
}