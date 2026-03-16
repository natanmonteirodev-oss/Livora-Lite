using System.ComponentModel.DataAnnotations;

namespace Livora_Lite.Domain.Entities
{
    public class Billing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ContractId { get; set; }

        [Required]
        [MaxLength(7)] // MM/YYYY
        public string Period { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int BillingStatusId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public Contract? Contract { get; set; }
        public BillingStatus? BillingStatus { get; set; }
        public ICollection<Payment>? Payments { get; set; }
    }
}