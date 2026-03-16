using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IMaintenanceRequestRepository
    {
        Task<MaintenanceRequest?> GetByIdAsync(int id);
        Task<IEnumerable<MaintenanceRequest>> GetAllAsync();
        Task<MaintenanceRequest> CreateAsync(MaintenanceRequest maintenanceRequest);
        Task<MaintenanceRequest> UpdateAsync(MaintenanceRequest maintenanceRequest);
        Task DeleteAsync(int id);
        Task<IEnumerable<MaintenanceRequest>> GetByPropertyAsync(int propertyId);
        Task<IEnumerable<MaintenanceRequest>> GetByContractAsync(int contractId);
        Task<IEnumerable<MaintenanceRequest>> GetByStatusAsync(MaintenanceStatus status);
        Task<IEnumerable<MaintenanceRequest>> GetByPriorityAsync(MaintenancePriority priority);
    }
}