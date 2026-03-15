namespace Livora_Lite.Application.DTO
{
    public class PropertyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public AddressDTO? Address { get; set; }
        public PropertyTypeDTO? PropertyType { get; set; }
        public PropertyStatusDTO? PropertyStatus { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int ParkingSpaces { get; set; }
        public decimal SuggestedRentValue { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}