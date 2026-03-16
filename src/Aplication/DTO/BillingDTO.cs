namespace Livora_Lite.Application.DTO
{
    public class BillingDTO
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string Period { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public int BillingStatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ContractDTO? Contract { get; set; }
        public BillingStatusDTO? BillingStatus { get; set; }
        public IEnumerable<PaymentDTO>? Payments { get; set; }

        // Computed property for display
        public string DisplayText => $"{Contract?.Property?.Name ?? "Propriedade não encontrada"} - {Contract?.Tenant?.Name ?? "Inquilino não encontrado"} - {Period}";
    }
}