using AutoMapper;
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
        private readonly IMapper _mapper;

        public TenantService(
            ITenantRepository tenantRepository,
            ITenantStatusRepository tenantStatusRepository,
            IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _tenantStatusRepository = tenantStatusRepository;
            _mapper = mapper;
        }

        public async Task<TenantDTO?> GetByIdAsync(int id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null) return null;

            return _mapper.Map<TenantDTO>(tenant);
        }

        public async Task<IEnumerable<TenantDTO>> GetAllAsync()
        {
            var tenants = await _tenantRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TenantDTO>>(tenants);
        }

        public async Task<TenantDTO> CreateAsync(CreateTenantRequestDTO request)
        {
            var tenant = _mapper.Map<Tenant>(request);
            var createdTenant = await _tenantRepository.CreateAsync(tenant);

            // Load related data
            createdTenant.TenantStatus = await _tenantStatusRepository.GetByIdAsync(request.TenantStatusId);

            return _mapper.Map<TenantDTO>(createdTenant);
        }

        public async Task<TenantDTO> UpdateAsync(UpdateTenantRequestDTO request)
        {
            var existingTenant = await _tenantRepository.GetByIdAsync(request.Id);
            if (existingTenant == null) throw new KeyNotFoundException("Tenant not found");

            _mapper.Map(request, existingTenant);
            existingTenant.UpdatedAt = DateTime.UtcNow;

            var updatedTenant = await _tenantRepository.UpdateAsync(existingTenant);

            // Load related data
            updatedTenant.TenantStatus = await _tenantStatusRepository.GetByIdAsync(request.TenantStatusId);

            return _mapper.Map<TenantDTO>(updatedTenant);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null) return false;

            await _tenantRepository.DeleteAsync(id);
            return true;
        }
    }
}