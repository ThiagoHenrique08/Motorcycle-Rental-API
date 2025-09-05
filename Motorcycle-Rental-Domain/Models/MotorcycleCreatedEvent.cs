namespace Motorcycle_Rental_Domain.Models
{
    public record MotorcycleCreatedEvent(string Identifier, int Year, string Model, string Plate);

}
