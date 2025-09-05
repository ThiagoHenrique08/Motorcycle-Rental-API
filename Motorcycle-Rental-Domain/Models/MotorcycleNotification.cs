namespace Motorcycle_Rental_Domain.Models
{
    public class MotorcycleNotification
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
