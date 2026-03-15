namespace Livora_Lite.Domain.Entities
{
    public class PropertyType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., Casa, Apartamento, Kitnet, Salão Comercial
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}