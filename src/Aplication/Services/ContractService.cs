using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite.Application.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IContractStatusRepository _contractStatusRepository;

        public ContractService(
            IContractRepository contractRepository,
            IPropertyRepository propertyRepository,
            ITenantRepository tenantRepository,
            IContractStatusRepository contractStatusRepository)
        {
            _contractRepository = contractRepository;
            _propertyRepository = propertyRepository;
            _tenantRepository = tenantRepository;
            _contractStatusRepository = contractStatusRepository;
        }

        public async Task<IEnumerable<ContractDTO>> GetAllContractsAsync()
        {
            var contracts = await _contractRepository.GetAllAsync();
            return contracts.Select(MapToDTO);
        }

        public async Task<ContractDTO?> GetByIdAsync(int id)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            return contract != null ? MapToDTO(contract) : null;
        }

        public async Task<ContractDTO> CreateAsync(CreateContractRequestDTO request)
        {
            // Validate that property exists
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId);
            if (property == null)
                throw new ArgumentException("Imóvel não encontrado");

            // Validate that tenant exists
            var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
            if (tenant == null)
                throw new ArgumentException("Inquilino não encontrado");

            // Validate that property doesn't have an active contract
            if (await _contractRepository.HasActiveContractForPropertyAsync(request.PropertyId))
                throw new InvalidOperationException("Este imóvel já possui um contrato ativo");

            // Validate contract status exists
            var status = await _contractStatusRepository.GetByIdAsync(request.ContractStatusId);
            if (status == null)
                throw new ArgumentException("Status do contrato não encontrado");

            var contract = new Contract
            {
                PropertyId = request.PropertyId,
                TenantId = request.TenantId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                RentValue = request.RentValue,
                DueDay = request.DueDay,
                LateFee = request.LateFee,
                InterestRate = request.InterestRate,
                SecurityDeposit = request.SecurityDeposit,
                ContractStatusId = request.ContractStatusId
            };

            var createdContract = await _contractRepository.CreateAsync(contract);
            return MapToDTO(createdContract);
        }

        public async Task UpdateAsync(UpdateContractRequestDTO request)
        {
            var existingContract = await _contractRepository.GetByIdAsync(request.Id);
            if (existingContract == null)
                throw new ArgumentException("Contrato não encontrado");

            // Validate that property exists
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId);
            if (property == null)
                throw new ArgumentException("Imóvel não encontrado");

            // Validate that tenant exists
            var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
            if (tenant == null)
                throw new ArgumentException("Inquilino não encontrado");

            // If changing property, validate that new property doesn't have active contract
            if (existingContract.PropertyId != request.PropertyId)
            {
                if (await _contractRepository.HasActiveContractForPropertyAsync(request.PropertyId))
                    throw new InvalidOperationException("Este imóvel já possui um contrato ativo");
            }

            // Validate contract status exists
            var status = await _contractStatusRepository.GetByIdAsync(request.ContractStatusId);
            if (status == null)
                throw new ArgumentException("Status do contrato não encontrado");

            existingContract.PropertyId = request.PropertyId;
            existingContract.TenantId = request.TenantId;
            existingContract.StartDate = request.StartDate;
            existingContract.EndDate = request.EndDate;
            existingContract.RentValue = request.RentValue;
            existingContract.DueDay = request.DueDay;
            existingContract.LateFee = request.LateFee;
            existingContract.InterestRate = request.InterestRate;
            existingContract.SecurityDeposit = request.SecurityDeposit;
            existingContract.ContractStatusId = request.ContractStatusId;
            existingContract.UpdatedAt = DateTime.UtcNow;

            await _contractRepository.UpdateAsync(existingContract);
        }

        public async Task DeleteAsync(int id)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            if (contract == null)
                throw new ArgumentException("Contrato não encontrado");

            await _contractRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ContractDTO>> GetActiveContractsAsync()
        {
            var contracts = await _contractRepository.GetActiveContractsAsync();
            return contracts.Select(MapToDTO);
        }

        public async Task<IEnumerable<ContractDTO>> GetContractsByPropertyAsync(int propertyId)
        {
            var contracts = await _contractRepository.GetContractsByPropertyAsync(propertyId);
            return contracts.Select(MapToDTO);
        }

        public async Task<IEnumerable<ContractDTO>> GetContractsByTenantAsync(int tenantId)
        {
            var contracts = await _contractRepository.GetContractsByTenantAsync(tenantId);
            return contracts.Select(MapToDTO);
        }

        public async Task<bool> HasActiveContractForPropertyAsync(int propertyId)
        {
            return await _contractRepository.HasActiveContractForPropertyAsync(propertyId);
        }

        private ContractDTO MapToDTO(Contract contract)
        {
            return new ContractDTO
            {
                Id = contract.Id,
                PropertyId = contract.PropertyId,
                TenantId = contract.TenantId,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                RentValue = contract.RentValue,
                DueDay = contract.DueDay,
                LateFee = contract.LateFee,
                InterestRate = contract.InterestRate,
                SecurityDeposit = contract.SecurityDeposit,
                ContractStatusId = contract.ContractStatusId,
                CreatedAt = contract.CreatedAt,
                UpdatedAt = contract.UpdatedAt,
                Property = contract.Property != null ? new PropertyDTO
                {
                    Id = contract.Property.Id,
                    Name = contract.Property.Name
                } : null,
                Tenant = contract.Tenant != null ? new TenantDTO
                {
                    Id = contract.Tenant.Id,
                    FirstName = contract.Tenant.FirstName,
                    LastName = contract.Tenant.LastName
                } : null,
                ContractStatus = contract.ContractStatus != null ? new ContractStatusDTO
                {
                    Id = contract.ContractStatus.Id,
                    Name = contract.ContractStatus.Name,
                    CreatedAt = contract.ContractStatus.CreatedAt
                } : null
            };
        }
    }
}