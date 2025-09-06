namespace Motorcycle_Rental_Infrastructure.Interfaces
{
    public interface IStorageService
    {
        Task SaveFileAsync(Stream fileStream, string fileName, string contentType);
    }
}
