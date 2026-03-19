namespace Livora_Lite.Application.DTO
{
    public class ContractDTO
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal RentValue { get; set; }
        public int DueDay { get; set; }
        public decimal LateFee { get; set; }
        public decimal InterestRate { get; set; }
        public decimal SecurityDeposit { get; set; }
        public int ContractStatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public PropertyDTO? Property { get; set; }
        public TenantDTO? Tenant { get; set; }
        public ContractStatusDTO? ContractStatus { get; set; }

        public string DisplayText => $"Contrato #{Id} - {Tenant?.Name} - R$ {RentValue:F2}";
    }
}