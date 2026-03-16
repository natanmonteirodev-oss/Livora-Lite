using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite.Application.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantStatusRepository _tenantStatusRepository;

        public TenantService(
            ITenantRepository tenantRepository,
            ITenantStatusRepository tenantStatusRepository)
        {
            _tenantRepository = tenantRepository;
            _tenantStatusRepository = tenantStatusRepository;
        }

        public async Task<TenantDTO?> GetByIdAsync(int id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null) return null;

            return MapToDTO(tenant);
        }

        public async Task<IEnumerable<TenantDTO>> GetAllAsync()
        {
            var tenants = await _tenantRepository.GetAllAsync();
            return tenants.Select(MapToDTO);
        }

        public async Task<TenantDTO> CreateAsync(CreateTenantRequestDTO request)
        {
            var tenant = new Tenant
            {
                Name = request.Name,
                Document = request.Document,
                Phone = request.Phone,
                Email = request.Email,
                CurrentAddress = request.CurrentAddress,
                TenantStatusId = request.TenantStatusId
            };
            var createdTenant = await _tenantRepository.CreateAsync(tenant);

            // Load related data
            createdTenant.TenantStatus = await _tenantStatusRepository.GetByIdAsync(request.TenantStatusId);

            return MapToDTO(createdTenant);
        }

        public async Task<TenantDTO> UpdateAsync(UpdateTenantRequestDTO request)
        {
            var existingTenant = await _tenantRepository.GetByIdAsync(request.Id);
            if (existingTenant == null) throw new KeyNotFoundException("Tenant not found");

            existingTenant.Name = request.Name;
            existingTenant.Document = request.Document;
            existingTenant.Phone = request.Phone;
            existingTenant.Email = request.Email;
            existingTenant.CurrentAddress = request.CurrentAddress;
            existingTenant.TenantStatusId = request.TenantStatusId;
            existingTenant.UpdatedAt = DateTime.UtcNow;

            var updatedTenant = await _tenantRepository.UpdateAsync(existingTenant);

            // Load related data
            updatedTenant.TenantStatus = await _tenantStatusRepository.GetByIdAsync(request.TenantStatusId);

            return MapToDTO(updatedTenant);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null) return false;

            await _tenantRepository.DeleteAsync(id);
            return true;
        }

        private TenantDTO MapToDTO(Tenant tenant)
        {
            return new TenantDTO
            {
                Id = tenant.Id,
                Name = tenant.Name,
                Document = tenant.Document,
                Phone = tenant.Phone,
                Email = tenant.Email,
                CurrentAddress = tenant.CurrentAddress,
                TenantStatus = tenant.TenantStatus != null ? new TenantStatusDTO
                {
                    Id = tenant.TenantStatus.Id,
                    Name = tenant.TenantStatus.Name
                } : null
            };
        }
    }
}