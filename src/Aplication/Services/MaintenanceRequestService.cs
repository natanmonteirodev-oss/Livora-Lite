using AutoMapper;
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
        private readonly IMapper _mapper;

        public MaintenanceRequestService(
            IMaintenanceRequestRepository maintenanceRequestRepository,
            IPropertyRepository propertyRepository,
            IContractRepository contractRepository,
            IMapper mapper)
        {
            _maintenanceRequestRepository = maintenanceRequestRepository;
            _propertyRepository = propertyRepository;
            _contractRepository = contractRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MaintenanceRequestDTO>> GetAllMaintenanceRequestsAsync()
        {
            var maintenanceRequests = await _maintenanceRequestRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MaintenanceRequestDTO>>(maintenanceRequests);
        }

        public async Task<MaintenanceRequestDTO?> GetMaintenanceRequestByIdAsync(int id)
        {
            var maintenanceRequest = await _maintenanceRequestRepository.GetByIdAsync(id);
            return maintenanceRequest != null ? _mapper.Map<MaintenanceRequestDTO>(maintenanceRequest) : null;
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

            var maintenanceRequest = _mapper.Map<MaintenanceRequest>(request);
            var createdMaintenanceRequest = await _maintenanceRequestRepository.CreateAsync(maintenanceRequest);
            return _mapper.Map<MaintenanceRequestDTO>(createdMaintenanceRequest);
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

            _mapper.Map(request, existingMaintenanceRequest);
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
            return _mapper.Map<IEnumerable<MaintenanceRequestDTO>>(maintenanceRequests);
        }

        public async Task<IEnumerable<MaintenanceRequestDTO>> GetMaintenanceRequestsByContractAsync(int contractId)
        {
            var maintenanceRequests = await _maintenanceRequestRepository.GetByContractAsync(contractId);
            return _mapper.Map<IEnumerable<MaintenanceRequestDTO>>(maintenanceRequests);
        }

        public async Task<IEnumerable<MaintenanceRequestDTO>> GetMaintenanceRequestsByStatusAsync(MaintenanceStatus status)
        {
            var maintenanceRequests = await _maintenanceRequestRepository.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<MaintenanceRequestDTO>>(maintenanceRequests);
        }

        public async Task<IEnumerable<MaintenanceRequestDTO>> GetMaintenanceRequestsByPriorityAsync(MaintenancePriority priority)
        {
            var maintenanceRequests = await _maintenanceRequestRepository.GetByPriorityAsync(priority);
            return _mapper.Map<IEnumerable<MaintenanceRequestDTO>>(maintenanceRequests);
        }
    }
}