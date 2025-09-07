namespace Motorcycle_Rental_Application.DTOs.LocationDTO
{
    public class UpdateLocationDTO(DateTime returnDate)
    {
        public DateTime ReturnDate { get; set; } = returnDate;
    }
}
