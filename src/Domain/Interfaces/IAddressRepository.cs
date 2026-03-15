using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Domain.Interfaces
{
    public interface IAddressRepository
    {
        Task<Address?> GetByIdAsync(int id);
        Task<IEnumerable<Address>> GetAllAsync();
        Task<Address> CreateAsync(Address address);
        Task<Address> UpdateAsync(Address address);
        Task<bool> DeleteAsync(int id);
    }
}