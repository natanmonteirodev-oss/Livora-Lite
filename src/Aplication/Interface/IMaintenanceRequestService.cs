using Livora_Lite.Application.DTO;
using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Application.Interface
{
    public interface IMaintenanceRequestService
    {
        Task<IEnumerable<MaintenanceRequestDTO>> GetAllMaintenanceRequestsAsync();
        Task<MaintenanceRequestDTO?> GetMaintenanceRequestByIdAsync(int id);
        Task<MaintenanceRequestDTO> CreateMaintenanceRequestAsync(CreateMaintenanceRequestDTO request);
        Task UpdateMaintenanceRequestAsync(UpdateMaintenanceRequestDTO request);
        Task DeleteMaintenanceRequestAsync(int id);
        Task<IEnumerable<MaintenanceRequestDTO>> GetMaintenanceRequestsByPropertyAsync(int propertyId);
        Task<IEnumerable<MaintenanceRequestDTO>> GetMaintenanceRequestsByContractAsync(int contractId);
        Task<IEnumerable<MaintenanceRequestDTO>> GetMaintenanceRequestsByStatusAsync(MaintenanceStatus status);
        Task<IEnumerable<MaintenanceRequestDTO>> GetMaintenanceRequestsByPriorityAsync(MaintenancePriority priority);
    }
}