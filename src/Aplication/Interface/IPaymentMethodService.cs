using Livora_Lite.Application.DTO;

namespace Livora_Lite.Application.Interface
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodDTO>> GetAllPaymentMethodsAsync();
        Task<PaymentMethodDTO?> GetPaymentMethodByIdAsync(int id);
    }
}