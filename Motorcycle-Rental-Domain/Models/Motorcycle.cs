namespace Motorcycle_Rental_Domain.Models
{
    public class Motorcycle
    {
        public string Identifier { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
        public Location Location { get; set; }
    }
}
