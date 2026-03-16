using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure
{
    public class TenantStatusRepository : ITenantStatusRepository
    {
        private readonly LivoraDbContext _context;

        public TenantStatusRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<TenantStatus?> GetByIdAsync(int id)
        {
            return await _context.TenantStatuses.FirstOrDefaultAsync(ts => ts.Id == id && ts.IsActive);
        }

        public async Task<IEnumerable<TenantStatus>> GetAllAsync()
        {
            return await _context.TenantStatuses.Where(ts => ts.IsActive).ToListAsync();
        }

        public async Task<TenantStatus> CreateAsync(TenantStatus tenantStatus)
        {
            tenantStatus.CreatedAt = DateTime.UtcNow;
            _context.TenantStatuses.Add(tenantStatus);
            await _context.SaveChangesAsync();
            return tenantStatus;
        }

        public async Task<TenantStatus> UpdateAsync(TenantStatus tenantStatus)
        {
            _context.TenantStatuses.Update(tenantStatus);
            await _context.SaveChangesAsync();
            return tenantStatus;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tenantStatus = await GetByIdAsync(id);
            if (tenantStatus == null) return false;

            tenantStatus.IsActive = false;
            await UpdateAsync(tenantStatus);
            return true;
        }
    }
}