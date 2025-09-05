namespace Motorcycle_Rental_Application.DTOs.MotorcycleDTO
{
    public class GetMotorcyclePerPlateDTO(string plate)
    {
        public string Plate { get; set; } = plate;
    }
}
