using System.ComponentModel.DataAnnotations;

namespace Livora_Lite.Domain.Entities
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PropertyId { get; set; }

        [Required]
        public int TenantId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal RentValue { get; set; }

        [Required]
        [Range(1, 31)]
        public int DueDay { get; set; }

        [Range(0, double.MaxValue)]
        public decimal LateFee { get; set; }

        [Range(0, double.MaxValue)]
        public decimal InterestRate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal SecurityDeposit { get; set; }

        [Required]
        public int ContractStatusId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public Property? Property { get; set; }
        public Tenant? Tenant { get; set; }
        public ContractStatus? ContractStatus { get; set; }
    }
}