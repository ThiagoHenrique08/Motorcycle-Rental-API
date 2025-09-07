using Motorcycle_Rental_Domain.Models;

namespace Motorcycle_Rental_Infrastructure.Interfaces
{
    public interface IDeliveryManRepository : IDAL<DeliveryMan>
    {
        Task<bool> ExistsByCNPJAsync(string cnpj);
        Task<bool> ExistsByCNHAsync(string cnh);
    }
}
