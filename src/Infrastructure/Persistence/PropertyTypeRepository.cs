using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Livora_Lite.Infrastructure
{
    public class PropertyTypeRepository : IPropertyTypeRepository
    {
        private readonly LivoraDbContext _context;

        public PropertyTypeRepository(LivoraDbContext context)
        {
            _context = context;
        }

        public async Task<PropertyType?> GetByIdAsync(int id)
        {
            return await _context.PropertyTypes.FirstOrDefaultAsync(pt => pt.Id == id && pt.IsActive);
        }

        public async Task<IEnumerable<PropertyType>> GetAllAsync()
        {
            return await _context.PropertyTypes.Where(pt => pt.IsActive).ToListAsync();
        }

        public async Task<PropertyType> CreateAsync(PropertyType propertyType)
        {
            propertyType.CreatedAt = DateTime.UtcNow;
            _context.PropertyTypes.Add(propertyType);
            await _context.SaveChangesAsync();
            return propertyType;
        }

        public async Task<PropertyType> UpdateAsync(PropertyType propertyType)
        {
            _context.PropertyTypes.Update(propertyType);
            await _context.SaveChangesAsync();
            return propertyType;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var propertyType = await GetByIdAsync(id);
            if (propertyType == null) return false;

            propertyType.IsActive = false;
            await UpdateAsync(propertyType);
            return true;
        }
    }
}