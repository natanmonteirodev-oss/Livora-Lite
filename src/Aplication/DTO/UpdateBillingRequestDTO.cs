using System.ComponentModel.DataAnnotations;

namespace Livora_Lite.Application.DTO
{
    public class UpdateBillingRequestDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Contrato é obrigatório")]
        public int ContractId { get; set; }

        [Required(ErrorMessage = "Período é obrigatório")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{4}$", ErrorMessage = "Período deve estar no formato MM/YYYY")]
        public string Period { get; set; } = string.Empty;

        [Required(ErrorMessage = "Valor é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Data de vencimento é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public int BillingStatusId { get; set; }
    }
}