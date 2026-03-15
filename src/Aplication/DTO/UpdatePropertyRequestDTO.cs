namespace Livora_Lite.Application.DTO
{
    public class UpdatePropertyRequestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public UpdateAddressRequestDTO Address { get; set; } = new();
        public int PropertyTypeId { get; set; }
        public int PropertyStatusId { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int ParkingSpaces { get; set; }
        public decimal SuggestedRentValue { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}