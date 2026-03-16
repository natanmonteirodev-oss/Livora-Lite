using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int id);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment> UpdateAsync(Payment payment);
        Task DeleteAsync(int id);
        Task<IEnumerable<Payment>> GetPaymentsByBillingAsync(int billingId);
        Task<decimal> GetTotalPaidForBillingAsync(int billingId);
    }
}