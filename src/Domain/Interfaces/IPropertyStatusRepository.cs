using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IPropertyStatusRepository
    {
        Task<PropertyStatus?> GetByIdAsync(int id);
        Task<IEnumerable<PropertyStatus>> GetAllAsync();
        Task<PropertyStatus> CreateAsync(PropertyStatus propertyStatus);
        Task<PropertyStatus> UpdateAsync(PropertyStatus propertyStatus);
        Task<bool> DeleteAsync(int id);
    }
}