namespace Motorcycle_Rental_Domain.Models
{
    public class Motorcycle
    {
        public string Identifier { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty;
    }
}
