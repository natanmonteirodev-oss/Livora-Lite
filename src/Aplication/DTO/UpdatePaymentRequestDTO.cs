using System.ComponentModel.DataAnnotations;

namespace Livora_Lite.Application.DTO
{
    public class UpdatePaymentRequestDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Cobrança é obrigatória")]
        public int BillingId { get; set; }

        [Required(ErrorMessage = "Data do pagamento é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Valor pago é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal AmountPaid { get; set; }

        [Required(ErrorMessage = "Método de pagamento é obrigatório")]
        public int PaymentMethodId { get; set; }
    }
}