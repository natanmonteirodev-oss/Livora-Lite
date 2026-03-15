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

        public PropertyService(
            IPropertyRepository propertyRepository,
            IAddressRepository addressRepository,
            IPropertyTypeRepository propertyTypeRepository,
            IPropertyStatusRepository propertyStatusRepository)
        {
            _propertyRepository = propertyRepository;
            _addressRepository = addressRepository;
            _propertyTypeRepository = propertyTypeRepository;
            _propertyStatusRepository = propertyStatusRepository;
        }

        public async Task<PropertyDTO?> GetByIdAsync(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null) return null;

            return MapToDTO(property);
        }

        public async Task<IEnumerable<PropertyDTO>> GetAllAsync()
        {
            var properties = await _propertyRepository.GetAllAsync();
            return properties.Select(MapToDTO);
        }

        public async Task<PropertyDTO> CreateAsync(CreatePropertyRequestDTO request)
        {
            // Create address
            var address = new Address
            {
                Street = request.Address.Street,
                Number = request.Address.Number,
                Complement = request.Address.Complement,
                Neighborhood = request.Address.Neighborhood,
                City = request.Address.City,
                State = request.Address.State,
                ZipCode = request.Address.ZipCode,
                Country = request.Address.Country
            };
            var createdAddress = await _addressRepository.CreateAsync(address);

            // Create property
            var property = new Property
            {
                Name = request.Name,
                AddressId = createdAddress.Id,
                PropertyTypeId = request.PropertyTypeId,
                PropertyStatusId = request.PropertyStatusId,
                Area = request.Area,
                Bedrooms = request.Bedrooms,
                Bathrooms = request.Bathrooms,
                ParkingSpaces = request.ParkingSpaces,
                SuggestedRentValue = request.SuggestedRentValue,
                Description = request.Description
            };
            var createdProperty = await _propertyRepository.CreateAsync(property);

            // Load related data
            createdProperty.Address = createdAddress;
            createdProperty.PropertyType = await _propertyTypeRepository.GetByIdAsync(request.PropertyTypeId);
            createdProperty.PropertyStatus = await _propertyStatusRepository.GetByIdAsync(request.PropertyStatusId);

            return MapToDTO(createdProperty);
        }

        public async Task<PropertyDTO> UpdateAsync(UpdatePropertyRequestDTO request)
        {
            var existingProperty = await _propertyRepository.GetByIdAsync(request.Id);
            if (existingProperty == null) throw new KeyNotFoundException("Property not found");

            // Update address
            var address = await _addressRepository.GetByIdAsync(request.Address.Id);
            if (address != null)
            {
                address.Street = request.Address.Street;
                address.Number = request.Address.Number;
                address.Complement = request.Address.Complement;
                address.Neighborhood = request.Address.Neighborhood;
                address.City = request.Address.City;
                address.State = request.Address.State;
                address.ZipCode = request.Address.ZipCode;
                address.Country = request.Address.Country;
                await _addressRepository.UpdateAsync(address);
            }

            // Update property
            existingProperty.Name = request.Name;
            existingProperty.PropertyTypeId = request.PropertyTypeId;
            existingProperty.PropertyStatusId = request.PropertyStatusId;
            existingProperty.Area = request.Area;
            existingProperty.Bedrooms = request.Bedrooms;
            existingProperty.Bathrooms = request.Bathrooms;
            existingProperty.ParkingSpaces = request.ParkingSpaces;
            existingProperty.SuggestedRentValue = request.SuggestedRentValue;
            existingProperty.Description = request.Description;
            existingProperty.UpdatedAt = DateTime.UtcNow;

            var updatedProperty = await _propertyRepository.UpdateAsync(existingProperty);

            // Load related data
            updatedProperty.Address = address;
            updatedProperty.PropertyType = await _propertyTypeRepository.GetByIdAsync(request.PropertyTypeId);
            updatedProperty.PropertyStatus = await _propertyStatusRepository.GetByIdAsync(request.PropertyStatusId);

            return MapToDTO(updatedProperty);
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

        private PropertyDTO MapToDTO(Property property)
        {
            return new PropertyDTO
            {
                Id = property.Id,
                Name = property.Name,
                Address = property.Address != null ? new AddressDTO
                {
                    Id = property.Address.Id,
                    Street = property.Address.Street,
                    Number = property.Address.Number,
                    Complement = property.Address.Complement,
                    Neighborhood = property.Address.Neighborhood,
                    City = property.Address.City,
                    State = property.Address.State,
                    ZipCode = property.Address.ZipCode,
                    Country = property.Address.Country
                } : null,
                PropertyType = property.PropertyType != null ? new PropertyTypeDTO
                {
                    Id = property.PropertyType.Id,
                    Name = property.PropertyType.Name
                } : null,
                PropertyStatus = property.PropertyStatus != null ? new PropertyStatusDTO
                {
                    Id = property.PropertyStatus.Id,
                    Name = property.PropertyStatus.Name
                } : null,
                Area = property.Area,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                ParkingSpaces = property.ParkingSpaces,
                SuggestedRentValue = property.SuggestedRentValue,
                Description = property.Description
            };
        }
    }
}