using Livora_Lite.Application.DTO;

namespace Livora_Lite.Application.Interface
{
    public interface ITenantService
    {
        Task<TenantDTO?> GetByIdAsync(int id);
        Task<IEnumerable<TenantDTO>> GetAllAsync();
        Task<TenantDTO> CreateAsync(CreateTenantRequestDTO request);
        Task<TenantDTO> UpdateAsync(UpdateTenantRequestDTO request);
        Task<bool> DeleteAsync(int id);
    }
}