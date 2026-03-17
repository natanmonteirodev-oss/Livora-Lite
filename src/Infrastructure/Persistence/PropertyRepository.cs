using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly LivoraDbContext _context;

        public PropertyRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<Property?> GetByIdAsync(int id)
        {
            return await _context.Properties
                .Include(p => p.Address)
                .Include(p => p.PropertyType)
                .Include(p => p.PropertyStatus)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }

        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _context.Properties
                .Include(p => p.Address)
                .Include(p => p.PropertyType)
                .Include(p => p.PropertyStatus)
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetByOwnerIdAsync(int ownerId)
        {
            return await _context.Properties
                .Include(p => p.Address)
                .Include(p => p.PropertyType)
                .Include(p => p.PropertyStatus)
                .Include(p => p.Owner)
                .Where(p => p.OwnerId == ownerId && p.IsActive)
                .ToListAsync();
        }

        public async Task<Property> CreateAsync(Property property)
        {
            property.CreatedAt = DateTime.UtcNow;
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
            return property;
        }

        public async Task<Property> UpdateAsync(Property property)
        {
            property.UpdatedAt = DateTime.UtcNow;
            _context.Properties.Update(property);
            await _context.SaveChangesAsync();
            return property;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var property = await GetByIdAsync(id);
            if (property == null) return false;

            property.IsActive = false;
            await UpdateAsync(property);
            return true;
        }
    }
}