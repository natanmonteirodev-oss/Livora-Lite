using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IPropertyRepository
    {
        Task<Property?> GetByIdAsync(int id);
        Task<IEnumerable<Property>> GetAllAsync();
        Task<Property> CreateAsync(Property property);
        Task<Property> UpdateAsync(Property property);
        Task<bool> DeleteAsync(int id);
    }
}