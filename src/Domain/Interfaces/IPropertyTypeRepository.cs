using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IPropertyTypeRepository
    {
        Task<PropertyType?> GetByIdAsync(int id);
        Task<IEnumerable<PropertyType>> GetAllAsync();
        Task<PropertyType> CreateAsync(PropertyType propertyType);
        Task<PropertyType> UpdateAsync(PropertyType propertyType);
        Task<bool> DeleteAsync(int id);
    }
}