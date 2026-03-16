using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite.Application.Services
{
    public class MaintenanceRequestService : IMaintenanceRequestService
    {
        private readonly IMaintenanceRequestRepository _maintenanceRequestRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IContractRepository _contractRepository;

        public MaintenanceRequestService(
            IMaintenanceRequestRepository maintenanceRequestRepository,
            IPropertyRepository propertyRepository,
            IContractRepository contractRepository)
        {
            _maintenanceRequestRepository = maintenanceRequestRepository;
            _propertyRepository = propertyRepository;
            _contractRepository = contractRepository;
        }

        public async Task<IEnumerable<MaintenanceRequestDTO>> GetAllMaintenanceRequestsAsync()
        {
            var maintenanceRequests = await _maintenanceRequestRepository.GetAllAsync();
            return maintenanceRequests.Select(MapToDTO);
        }

        public async Task<MaintenanceRequestDTO?> GetMaintenanceRequestByIdAsync(int id)
        {
            var maintenanceRequest = await _maintenanceRequestRepository.GetByIdAsync(id);
            return maintenanceRequest != null ? MapToDTO(maintenanceRequest) : null;
        }

        public async Task<MaintenanceRequestDTO> CreateMaintenanceRequestAsync(CreateMaintenanceRequestDTO request)
        {
            // Validate that property exists
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId);
            if (property == null)
                throw new ArgumentException("Imóvel não encontrado");

            // Validate contract if provided
            if (request.ContractId.HasValue)
            {
                var contract = await _contractRepository.GetByIdAsync(request.ContractId.Value);
                if (contract == null)
                    throw new ArgumentException("Contrato não encontrado");
            }

            var maintenanceRequest = new MaintenanceRequest
            {
                PropertyId = request.PropertyId,
                ContractId = request.ContractId,
                Description = request.Description,
                RequestDate = request.RequestDate,
                Priority = request.Priority,
                Status = request.Status
            };

            var createdMaintenanceRequest = await _maintenanceRequestRepository.CreateAsync(maintenanceRequest);
            return MapToDTO(createdMaintenanceRequest);
        }

        public async Task UpdateMaintenanceRequestAsync(UpdateMaintenanceRequestDTO request)
        {
            var existingMaintenanceRequest = await _maintenanceRequestRepository.GetByIdAsync(request.Id);
            if (existingMaintenanceRequest == null)
                throw new ArgumentException("Solicitação de manutenção não encontrada");

            // Validate that property exists
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId);
            if (property == null)
                throw new ArgumentException("Imóvel não encontrado");

            // Validate contract if provided
            if (request.ContractId.HasValue)
            {
                var contract = await _contractRepository.GetByIdAsync(request.ContractId.Value);
                if (contract == null)
                    throw new ArgumentException("Contrato não encontrado");
            }

            existingMaintenanceRequest.PropertyId = request.PropertyId;
            existingMaintenanceRequest.ContractId = request.ContractId;
            existingMaintenanceRequest.Description = request.Description;
            existingMaintenanceRequest.RequestDate = request.RequestDate;
            existingMaintenanceRequest.Priority = request.Priority;
            existingMaintenanceRequest.Status = request.Status;

            await _maintenanceRequestRepository.UpdateAsync(existingMaintenanceRequest);
        }

        public async Task DeleteMaintenanceRequestAsync(int id)
        {
            var maintenanceRequest = await _maintenanceRequestRepository.GetByIdAsync(id);
            if (maintenanceRequest == null)
                throw new ArgumentException("Solicitação de manutenção não encontrada");

            await _maintenanceRequestRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<MaintenanceRequestDTO>> GetMaintenanceRequestsByPropertyAsync(int propertyId)
        {
            var maintenanceRequests = await _maintenanceRequestRepository.GetByPropertyAsync(propertyId);
            return maintenanceRequests.Select(MapToDTO);
        }

        public async Task<IEnumerable<MaintenanceRequestDTO>> GetMaintenanceRequestsByContractAsync(int contractId)
        {
            var maintenanceRequests = await _maintenanceRequestRepository.GetByContractAsync(contractId);
            return maintenanceRequests.Select(MapToDTO);
        }

        public async Task<IEnumerable<MaintenanceRequestDTO>> GetMaintenanceRequestsByStatusAsync(MaintenanceStatus status)
        {
            var maintenanceRequests = await _maintenanceRequestRepository.GetByStatusAsync(status);
            return maintenanceRequests.Select(MapToDTO);
        }

        public async Task<IEnumerable<MaintenanceRequestDTO>> GetMaintenanceRequestsByPriorityAsync(MaintenancePriority priority)
        {
            var maintenanceRequests = await _maintenanceRequestRepository.GetByPriorityAsync(priority);
            return maintenanceRequests.Select(MapToDTO);
        }

        private MaintenanceRequestDTO MapToDTO(MaintenanceRequest maintenanceRequest)
        {
            return new MaintenanceRequestDTO
            {
                Id = maintenanceRequest.Id,
                PropertyId = maintenanceRequest.PropertyId,
                ContractId = maintenanceRequest.ContractId,
                Description = maintenanceRequest.Description,
                RequestDate = maintenanceRequest.RequestDate,
                Priority = maintenanceRequest.Priority,
                Status = maintenanceRequest.Status,
                CreatedAt = maintenanceRequest.CreatedAt,
                Property = maintenanceRequest.Property != null ? new PropertyDTO
                {
                    Id = maintenanceRequest.Property.Id,
                    Name = maintenanceRequest.Property.Name
                } : null,
                Contract = maintenanceRequest.Contract != null ? new ContractDTO
                {
                    Id = maintenanceRequest.Contract.Id,
                    Tenant = maintenanceRequest.Contract.Tenant != null ? new TenantDTO
                    {
                        Id = maintenanceRequest.Contract.Tenant.Id,
                        Name = maintenanceRequest.Contract.Tenant.Name
                    } : null
                } : null
            };
        }
    }
}