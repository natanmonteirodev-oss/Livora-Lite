using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite.Application.Services
{
    public class BillingService : IBillingService
    {
        private readonly IBillingRepository _billingRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IBillingStatusRepository _billingStatusRepository;

        public BillingService(
            IBillingRepository billingRepository,
            IContractRepository contractRepository,
            IBillingStatusRepository billingStatusRepository)
        {
            _billingRepository = billingRepository;
            _contractRepository = contractRepository;
            _billingStatusRepository = billingStatusRepository;
        }

        public async Task<IEnumerable<BillingDTO>> GetAllBillingsAsync()
        {
            var billings = await _billingRepository.GetAllAsync();
            return billings.Select(MapToDTO);
        }

        public async Task<BillingDTO?> GetBillingByIdAsync(int id)
        {
            var billing = await _billingRepository.GetByIdAsync(id);
            return billing != null ? MapToDTO(billing) : null;
        }

        public async Task<BillingDTO> CreateBillingAsync(CreateBillingRequestDTO request)
        {
            // Validate that contract exists
            var contract = await _contractRepository.GetByIdAsync(request.ContractId);
            if (contract == null)
                throw new ArgumentException("Contrato não encontrado");

            // Validate that billing status exists
            var status = await _billingStatusRepository.GetByIdAsync(request.BillingStatusId);
            if (status == null)
                throw new ArgumentException("Status da cobrança não encontrado");

            // Validate that there's no existing billing for this contract and period
            if (await _billingRepository.HasBillingForContractAndPeriodAsync(request.ContractId, request.Period))
                throw new InvalidOperationException("Já existe uma cobrança para este contrato neste período");

            var billing = new Billing
            {
                ContractId = request.ContractId,
                Period = request.Period,
                Amount = request.Amount,
                DueDate = request.DueDate,
                BillingStatusId = request.BillingStatusId
            };

            var createdBilling = await _billingRepository.CreateAsync(billing);
            return MapToDTO(createdBilling);
        }

        public async Task UpdateBillingAsync(UpdateBillingRequestDTO request)
        {
            var existingBilling = await _billingRepository.GetByIdAsync(request.Id);
            if (existingBilling == null)
                throw new ArgumentException("Cobrança não encontrada");

            // Validate that contract exists
            var contract = await _contractRepository.GetByIdAsync(request.ContractId);
            if (contract == null)
                throw new ArgumentException("Contrato não encontrado");

            // Validate that billing status exists
            var status = await _billingStatusRepository.GetByIdAsync(request.BillingStatusId);
            if (status == null)
                throw new ArgumentException("Status da cobrança não encontrado");

            // If changing contract or period, validate uniqueness
            if (existingBilling.ContractId != request.ContractId || existingBilling.Period != request.Period)
            {
                if (await _billingRepository.HasBillingForContractAndPeriodAsync(request.ContractId, request.Period))
                    throw new InvalidOperationException("Já existe uma cobrança para este contrato neste período");
            }

            existingBilling.ContractId = request.ContractId;
            existingBilling.Period = request.Period;
            existingBilling.Amount = request.Amount;
            existingBilling.DueDate = request.DueDate;
            existingBilling.BillingStatusId = request.BillingStatusId;
            existingBilling.UpdatedAt = DateTime.UtcNow;

            await _billingRepository.UpdateAsync(existingBilling);
        }

        public async Task DeleteBillingAsync(int id)
        {
            var billing = await _billingRepository.GetByIdAsync(id);
            if (billing == null)
                throw new ArgumentException("Cobrança não encontrada");

            await _billingRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<BillingDTO>> GetBillingsByContractAsync(int contractId)
        {
            var billings = await _billingRepository.GetBillingsByContractAsync(contractId);
            return billings.Select(MapToDTO);
        }

        public async Task<IEnumerable<BillingDTO>> GetBillingsByPeriodAsync(string period)
        {
            var billings = await _billingRepository.GetBillingsByPeriodAsync(period);
            return billings.Select(MapToDTO);
        }

        public async Task<bool> HasBillingForContractAndPeriodAsync(int contractId, string period)
        {
            return await _billingRepository.HasBillingForContractAndPeriodAsync(contractId, period);
        }

        private BillingDTO MapToDTO(Billing billing)
        {
            return new BillingDTO
            {
                Id = billing.Id,
                ContractId = billing.ContractId,
                Period = billing.Period,
                Amount = billing.Amount,
                DueDate = billing.DueDate,
                BillingStatusId = billing.BillingStatusId,
                CreatedAt = billing.CreatedAt,
                UpdatedAt = billing.UpdatedAt,
                Contract = billing.Contract != null ? new ContractDTO
                {
                    Id = billing.Contract.Id,
                    PropertyId = billing.Contract.PropertyId,
                    TenantId = billing.Contract.TenantId,
                    StartDate = billing.Contract.StartDate,
                    RentValue = billing.Contract.RentValue,
                    Property = billing.Contract.Property != null ? new PropertyDTO
                    {
                        Id = billing.Contract.Property.Id,
                        Name = billing.Contract.Property.Name
                    } : null,
                    Tenant = billing.Contract.Tenant != null ? new TenantDTO
                    {
                        Id = billing.Contract.Tenant.Id,
                        FirstName = billing.Contract.Tenant.FirstName,
                        LastName = billing.Contract.Tenant.LastName
                    } : null
                } : null,
                BillingStatus = billing.BillingStatus != null ? new BillingStatusDTO
                {
                    Id = billing.BillingStatus.Id,
                    Name = billing.BillingStatus.Name,
                    CreatedAt = billing.BillingStatus.CreatedAt
                } : null,
                Payments = billing.Payments?.Select(p => new PaymentDTO
                {
                    Id = p.Id,
                    BillingId = p.BillingId,
                    PaymentDate = p.PaymentDate,
                    AmountPaid = p.AmountPaid,
                    PaymentMethodId = p.PaymentMethodId
                })
            };
        }
    }
}