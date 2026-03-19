using AutoMapper;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBillingRepository _billingRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IMapper _mapper;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IBillingRepository billingRepository,
            IPaymentMethodRepository paymentMethodRepository,
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _billingRepository = billingRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentDTO>>(payments);
        }

        public async Task<PaymentDTO?> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return payment != null ? _mapper.Map<PaymentDTO>(payment) : null;
        }

        public async Task<PaymentDTO> CreatePaymentAsync(CreatePaymentRequestDTO request)
        {
            // Validate that billing exists
            var billing = await _billingRepository.GetByIdAsync(request.BillingId);
            if (billing == null)
                throw new ArgumentException("Cobrança não encontrada");

            // Validate that payment method exists
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(request.PaymentMethodId);
            if (paymentMethod == null)
                throw new ArgumentException("Método de pagamento não encontrado");

            var payment = _mapper.Map<Payment>(request);
            var createdPayment = await _paymentRepository.CreateAsync(payment);
            return _mapper.Map<PaymentDTO>(createdPayment);
        }

        public async Task UpdatePaymentAsync(UpdatePaymentRequestDTO request)
        {
            var existingPayment = await _paymentRepository.GetByIdAsync(request.Id);
            if (existingPayment == null)
                throw new ArgumentException("Pagamento não encontrado");

            // Validate that billing exists
            var billing = await _billingRepository.GetByIdAsync(request.BillingId);
            if (billing == null)
                throw new ArgumentException("Cobrança não encontrada");

            // Validate that payment method exists
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(request.PaymentMethodId);
            if (paymentMethod == null)
                throw new ArgumentException("Método de pagamento não encontrado");

            _mapper.Map(request, existingPayment);
            existingPayment.UpdatedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(existingPayment);
        }

        public async Task DeletePaymentAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
                throw new ArgumentException("Pagamento não encontrado");

            await _paymentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PaymentDTO>> GetPaymentsByBillingAsync(int billingId)
        {
            var payments = await _paymentRepository.GetPaymentsByBillingAsync(billingId);
            return _mapper.Map<IEnumerable<PaymentDTO>>(payments);
        }

        public async Task<decimal> GetTotalPaidForBillingAsync(int billingId)
        {
            return await _paymentRepository.GetTotalPaidForBillingAsync(billingId);
        }

    }
}