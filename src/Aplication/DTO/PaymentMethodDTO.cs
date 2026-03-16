namespace Livora_Lite.Application.DTO
{
    public class PaymentMethodDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}