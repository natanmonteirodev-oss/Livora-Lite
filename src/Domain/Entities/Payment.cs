using System.ComponentModel.DataAnnotations;

namespace Livora_Lite.Domain.Entities
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BillingId { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal AmountPaid { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public Billing? Billing { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
}