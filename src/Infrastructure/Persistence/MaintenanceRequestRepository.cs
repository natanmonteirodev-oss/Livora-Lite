using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure
{
    public class MaintenanceRequestRepository : IMaintenanceRequestRepository
    {
        private readonly LivoraDbContext _context;

        public MaintenanceRequestRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<MaintenanceRequest?> GetByIdAsync(int id)
        {
            return await _context.MaintenanceRequests
                .Include(mr => mr.Property)
                .Include(mr => mr.Contract)
                    .ThenInclude(c => c.Tenant)
                .FirstOrDefaultAsync(mr => mr.Id == id);
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetAllAsync()
        {
            return await _context.MaintenanceRequests
                .Include(mr => mr.Property)
                .Include(mr => mr.Contract)
                    .ThenInclude(c => c.Tenant)
                .OrderByDescending(mr => mr.CreatedAt)
                .ToListAsync();
        }

        public async Task<MaintenanceRequest> CreateAsync(MaintenanceRequest maintenanceRequest)
        {
            _context.MaintenanceRequests.Add(maintenanceRequest);
            await _context.SaveChangesAsync();
            return maintenanceRequest;
        }

        public async Task<MaintenanceRequest> UpdateAsync(MaintenanceRequest maintenanceRequest)
        {
            _context.MaintenanceRequests.Update(maintenanceRequest);
            await _context.SaveChangesAsync();
            return maintenanceRequest;
        }

        public async Task DeleteAsync(int id)
        {
            var maintenanceRequest = await GetByIdAsync(id);
            if (maintenanceRequest != null)
            {
                _context.MaintenanceRequests.Remove(maintenanceRequest);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetByPropertyAsync(int propertyId)
        {
            return await _context.MaintenanceRequests
                .Include(mr => mr.Property)
                .Include(mr => mr.Contract)
                    .ThenInclude(c => c.Tenant)
                .Where(mr => mr.PropertyId == propertyId)
                .OrderByDescending(mr => mr.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetByContractAsync(int contractId)
        {
            return await _context.MaintenanceRequests
                .Include(mr => mr.Property)
                .Include(mr => mr.Contract)
                    .ThenInclude(c => c.Tenant)
                .Where(mr => mr.ContractId == contractId)
                .OrderByDescending(mr => mr.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetByStatusAsync(MaintenanceStatus status)
        {
            return await _context.MaintenanceRequests
                .Include(mr => mr.Property)
                .Include(mr => mr.Contract)
                    .ThenInclude(c => c.Tenant)
                .Where(mr => mr.Status == status)
                .OrderByDescending(mr => mr.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetByPriorityAsync(MaintenancePriority priority)
        {
            return await _context.MaintenanceRequests
                .Include(mr => mr.Property)
                .Include(mr => mr.Contract)
                    .ThenInclude(c => c.Tenant)
                .Where(mr => mr.Priority == priority)
                .OrderByDescending(mr => mr.CreatedAt)
                .ToListAsync();
        }
    }
}