namespace Livora_Lite.Domain.Entities
{
    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Identificação, e.g., Casa 01, Casa Centro
        public int AddressId { get; set; }
        public Address? Address { get; set; }
        public int PropertyTypeId { get; set; }
        public PropertyType? PropertyType { get; set; }
        public int PropertyStatusId { get; set; }
        public PropertyStatus? PropertyStatus { get; set; }
        public decimal Area { get; set; } // Área em m²
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int ParkingSpaces { get; set; }
        public decimal SuggestedRentValue { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}