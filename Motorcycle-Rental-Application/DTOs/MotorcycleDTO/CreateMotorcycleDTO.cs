namespace Motorcycle_Rental_Application.DTOs.MotorcycleDTO
{
    public record class CreateMotorcycleDTO(string Identifier, int Year, string Model, string Plate)
    {
        public string Identifier { get; set; } = Identifier;
        public int Year { get; set; } = Year;
        public string Model { get; set; } = Model;
        public string Plate { get; set; } = Plate;
    }
}
