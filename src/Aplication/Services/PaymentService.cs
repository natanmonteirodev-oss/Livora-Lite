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

        public PaymentService(
            IPaymentRepository paymentRepository,
            IBillingRepository billingRepository,
            IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentRepository = paymentRepository;
            _billingRepository = billingRepository;
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return payments.Select(MapToDTO);
        }

        public async Task<PaymentDTO?> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return payment != null ? MapToDTO(payment) : null;
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

            var payment = new Payment
            {
                BillingId = request.BillingId,
                PaymentDate = request.PaymentDate,
                AmountPaid = request.AmountPaid,
                PaymentMethodId = request.PaymentMethodId
            };

            var createdPayment = await _paymentRepository.CreateAsync(payment);
            return MapToDTO(createdPayment);
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

            existingPayment.BillingId = request.BillingId;
            existingPayment.PaymentDate = request.PaymentDate;
            existingPayment.AmountPaid = request.AmountPaid;
            existingPayment.PaymentMethodId = request.PaymentMethodId;
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
            return payments.Select(MapToDTO);
        }

        public async Task<decimal> GetTotalPaidForBillingAsync(int billingId)
        {
            return await _paymentRepository.GetTotalPaidForBillingAsync(billingId);
        }

        private PaymentDTO MapToDTO(Payment payment)
        {
            return new PaymentDTO
            {
                Id = payment.Id,
                BillingId = payment.BillingId,
                PaymentDate = payment.PaymentDate,
                AmountPaid = payment.AmountPaid,
                PaymentMethodId = payment.PaymentMethodId,
                CreatedAt = payment.CreatedAt,
                UpdatedAt = payment.UpdatedAt,
                Billing = payment.Billing != null ? new BillingDTO
                {
                    Id = payment.Billing.Id,
                    ContractId = payment.Billing.ContractId,
                    Period = payment.Billing.Period,
                    Amount = payment.Billing.Amount,
                    DueDate = payment.Billing.DueDate
                } : null,
                PaymentMethod = payment.PaymentMethod != null ? new PaymentMethodDTO
                {
                    Id = payment.PaymentMethod.Id,
                    Name = payment.PaymentMethod.Name,
                    CreatedAt = payment.PaymentMethod.CreatedAt
                } : null
            };
        }
    }
}