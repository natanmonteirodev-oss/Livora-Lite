using Livora_Lite.Application.DTO;

namespace Livora_Lite.Application.Interface
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync();
        Task<PaymentDTO?> GetPaymentByIdAsync(int id);
        Task<PaymentDTO> CreatePaymentAsync(CreatePaymentRequestDTO request);
        Task UpdatePaymentAsync(UpdatePaymentRequestDTO request);
        Task DeletePaymentAsync(int id);
        Task<IEnumerable<PaymentDTO>> GetPaymentsByBillingAsync(int billingId);
        Task<decimal> GetTotalPaidForBillingAsync(int billingId);
    }
}