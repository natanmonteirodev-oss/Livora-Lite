using System.ComponentModel.DataAnnotations;

namespace Livora_Lite.Application.DTO
{
    public class UpdateContractRequestDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imóvel é obrigatório")]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "Inquilino é obrigatório")]
        public int TenantId { get; set; }

        [Required(ErrorMessage = "Data de início é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Valor do aluguel é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal RentValue { get; set; }

        [Required(ErrorMessage = "Dia de vencimento é obrigatório")]
        [Range(1, 31, ErrorMessage = "Dia deve estar entre 1 e 31")]
        public int DueDay { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Multa deve ser maior ou igual a zero")]
        public decimal LateFee { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Juros deve ser maior ou igual a zero")]
        public decimal InterestRate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Depósito caução deve ser maior ou igual a zero")]
        public decimal SecurityDeposit { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public int ContractStatusId { get; set; }
    }
}