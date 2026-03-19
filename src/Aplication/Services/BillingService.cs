using AutoMapper;
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
        private readonly IMapper _mapper;

        public BillingService(
            IBillingRepository billingRepository,
            IContractRepository contractRepository,
            IBillingStatusRepository billingStatusRepository,
            IMapper mapper)
        {
            _billingRepository = billingRepository;
            _contractRepository = contractRepository;
            _billingStatusRepository = billingStatusRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BillingDTO>> GetAllBillingsAsync()
        {
            var billings = await _billingRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BillingDTO>>(billings);
        }

        public async Task<BillingDTO?> GetBillingByIdAsync(int id)
        {
            var billing = await _billingRepository.GetByIdAsync(id);
            return billing != null ? _mapper.Map<BillingDTO>(billing) : null;
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

            var billing = _mapper.Map<Billing>(request);
            var createdBilling = await _billingRepository.CreateAsync(billing);
            return _mapper.Map<BillingDTO>(createdBilling);
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

            _mapper.Map(request, existingBilling);
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
            return _mapper.Map<IEnumerable<BillingDTO>>(billings);
        }

        public async Task<IEnumerable<BillingDTO>> GetBillingsByPeriodAsync(string period)
        {
            var billings = await _billingRepository.GetBillingsByPeriodAsync(period);
            return _mapper.Map<IEnumerable<BillingDTO>>(billings);
        }

        public async Task<bool> HasBillingForContractAndPeriodAsync(int contractId, string period)
        {
            return await _billingRepository.HasBillingForContractAndPeriodAsync(contractId, period);
        }

    }
}