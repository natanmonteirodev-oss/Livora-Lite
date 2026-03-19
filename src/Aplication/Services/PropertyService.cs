using AutoMapper;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;
using Livora_Lite.Domain.Entities;
using Livora_Lite.Domain.Interfaces;

namespace Livora_Lite.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IPropertyStatusRepository _propertyStatusRepository;
        private readonly IMapper _mapper;

        public PropertyService(
            IPropertyRepository propertyRepository,
            IAddressRepository addressRepository,
            IPropertyTypeRepository propertyTypeRepository,
            IPropertyStatusRepository propertyStatusRepository,
            IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _addressRepository = addressRepository;
            _propertyTypeRepository = propertyTypeRepository;
            _propertyStatusRepository = propertyStatusRepository;
            _mapper = mapper;
        }

        public async Task<PropertyDTO?> GetByIdAsync(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null) return null;

            return _mapper.Map<PropertyDTO>(property);
        }

        public async Task<IEnumerable<PropertyDTO>> GetAllAsync()
        {
            var properties = await _propertyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
        }

        public async Task<PropertyDTO> CreateAsync(CreatePropertyRequestDTO request)
        {
            // Create address
            var address = _mapper.Map<Address>(request.Address);
            var createdAddress = await _addressRepository.CreateAsync(address);

            // Create property
            var property = _mapper.Map<Property>(request);
            property.AddressId = createdAddress.Id;
            var createdProperty = await _propertyRepository.CreateAsync(property);

            // Load related data
            createdProperty.Address = createdAddress;
            createdProperty.PropertyType = await _propertyTypeRepository.GetByIdAsync(request.PropertyTypeId);
            createdProperty.PropertyStatus = await _propertyStatusRepository.GetByIdAsync(request.PropertyStatusId);

            return _mapper.Map<PropertyDTO>(createdProperty);
        }

        public async Task<PropertyDTO> UpdateAsync(UpdatePropertyRequestDTO request)
        {
            var existingProperty = await _propertyRepository.GetByIdAsync(request.Id);
            if (existingProperty == null) throw new KeyNotFoundException("Property not found");

            // Update address
            var address = await _addressRepository.GetByIdAsync(request.Address.Id);
            if (address != null)
            {
                _mapper.Map(request.Address, address);
                await _addressRepository.UpdateAsync(address);
            }

            // Update property
            _mapper.Map(request, existingProperty);
            existingProperty.UpdatedAt = DateTime.UtcNow;

            var updatedProperty = await _propertyRepository.UpdateAsync(existingProperty);

            // Load related data
            updatedProperty.Address = address;
            updatedProperty.PropertyType = await _propertyTypeRepository.GetByIdAsync(request.PropertyTypeId);
            updatedProperty.PropertyStatus = await _propertyStatusRepository.GetByIdAsync(request.PropertyStatusId);

            return _mapper.Map<PropertyDTO>(updatedProperty);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null) return false;

            // Delete property first
            await _propertyRepository.DeleteAsync(id);

            // Optionally delete address if not used elsewhere
            // For now, keep address

            return true;
        }
    }
}