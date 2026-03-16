using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IPaymentMethodRepository
    {
        Task<PaymentMethod?> GetByIdAsync(int id);
        Task<IEnumerable<PaymentMethod>> GetAllAsync();
        Task<PaymentMethod> CreateAsync(PaymentMethod paymentMethod);
        Task<PaymentMethod> UpdateAsync(PaymentMethod paymentMethod);
        Task DeleteAsync(int id);
    }
}