using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure
{
    public class PropertyStatusRepository : IPropertyStatusRepository
    {
        private readonly LivoraDbContext _context;

        public PropertyStatusRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<PropertyStatus?> GetByIdAsync(int id)
        {
            return await _context.PropertyStatuses.FirstOrDefaultAsync(ps => ps.Id == id && ps.IsActive);
        }

        public async Task<IEnumerable<PropertyStatus>> GetAllAsync()
        {
            return await _context.PropertyStatuses.Where(ps => ps.IsActive).ToListAsync();
        }

        public async Task<PropertyStatus> CreateAsync(PropertyStatus propertyStatus)
        {
            propertyStatus.CreatedAt = DateTime.UtcNow;
            _context.PropertyStatuses.Add(propertyStatus);
            await _context.SaveChangesAsync();
            return propertyStatus;
        }

        public async Task<PropertyStatus> UpdateAsync(PropertyStatus propertyStatus)
        {
            _context.PropertyStatuses.Update(propertyStatus);
            await _context.SaveChangesAsync();
            return propertyStatus;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var propertyStatus = await GetByIdAsync(id);
            if (propertyStatus == null) return false;

            propertyStatus.IsActive = false;
            await UpdateAsync(propertyStatus);
            return true;
        }
    }
}