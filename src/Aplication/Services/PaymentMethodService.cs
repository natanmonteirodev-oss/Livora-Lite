using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite.Application.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<IEnumerable<PaymentMethodDTO>> GetAllPaymentMethodsAsync()
        {
            var paymentMethods = await _paymentMethodRepository.GetAllAsync();
            return paymentMethods.Select(pm => new PaymentMethodDTO
            {
                Id = pm.Id,
                Name = pm.Name,
                CreatedAt = pm.CreatedAt
            });
        }

        public async Task<PaymentMethodDTO?> GetPaymentMethodByIdAsync(int id)
        {
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id);
            if (paymentMethod == null) return null;

            return new PaymentMethodDTO
            {
                Id = paymentMethod.Id,
                Name = paymentMethod.Name,
                CreatedAt = paymentMethod.CreatedAt
            };
        }
    }
}