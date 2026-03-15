using Livora_Lite.Application.DTO;

namespace Livora_Lite.Application.Interface
{
    public interface IPropertyService
    {
        Task<PropertyDTO?> GetByIdAsync(int id);
        Task<IEnumerable<PropertyDTO>> GetAllAsync();
        Task<PropertyDTO> CreateAsync(CreatePropertyRequestDTO request);
        Task<PropertyDTO> UpdateAsync(UpdatePropertyRequestDTO request);
        Task<bool> DeleteAsync(int id);
    }
}