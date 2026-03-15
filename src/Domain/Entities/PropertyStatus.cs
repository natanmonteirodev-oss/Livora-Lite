namespace Livora_Lite.Domain.Entities
{
    public class PropertyStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Disponível, Alugado, Manutenção, Reservado, Inativo
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}