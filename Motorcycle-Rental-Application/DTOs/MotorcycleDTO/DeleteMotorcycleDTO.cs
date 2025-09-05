namespace Motorcycle_Rental_Application.DTOs.MotorcycleDTO
{
    public record class DeleteMotorcycleDTO
    {
        public DeleteMotorcycleDTO(string id)
        {
            this.Id = id;
        }

        public string Id { get; set; }
    }
}
