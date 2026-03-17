using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IPropertyRepository
    {
        Task<Property?> GetByIdAsync(int id);
        Task<IEnumerable<Property>> GetAllAsync();
        Task<IEnumerable<Property>> GetByOwnerIdAsync(int ownerId);  // ← Novo método
        Task<Property> CreateAsync(Property property);
        Task<Property> UpdateAsync(Property property);
        Task<bool> DeleteAsync(int id);
    }
}