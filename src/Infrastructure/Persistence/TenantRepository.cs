using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure
{
    public class TenantRepository : ITenantRepository
    {
        private readonly LivoraDbContext _context;

        public TenantRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<Tenant?> GetByIdAsync(int id)
        {
            return await _context.Tenants
                .Include(t => t.TenantStatus)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive);
        }

        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return await _context.Tenants
                .Include(t => t.TenantStatus)
                .Where(t => t.IsActive)
                .ToListAsync();
        }

        public async Task<Tenant?> GetByUserIdAsync(int userId)
        {
            return await _context.Tenants
                .Include(t => t.TenantStatus)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.UserId == userId && t.IsActive);
        }

        public async Task<Tenant> CreateAsync(Tenant tenant)
        {
            tenant.CreatedAt = DateTime.UtcNow;
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();
            return tenant;
        }

        public async Task<Tenant> UpdateAsync(Tenant tenant)
        {
            tenant.UpdatedAt = DateTime.UtcNow;
            _context.Tenants.Update(tenant);
            await _context.SaveChangesAsync();
            return tenant;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tenant = await GetByIdAsync(id);
            if (tenant == null) return false;

            tenant.IsActive = false;
            await UpdateAsync(tenant);
            return true;
        }
    }
}