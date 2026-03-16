namespace Livora_Lite.Application.DTO
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public int BillingId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public BillingDTO? Billing { get; set; }
        public PaymentMethodDTO? PaymentMethod { get; set; }
    }
}