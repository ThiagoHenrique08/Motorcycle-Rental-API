using Motorcycle_Rental_Application.DTOs.DeliveryManDTO;
using Motorcycle_Rental_Domain.Models;

namespace Motorcycle_Rental_Application.Interfaces.DeliveryManInterfaces
{
    public interface ICreateDeliveryManUseCase : IUseCase<CreateDeliveryManDTO, DeliveryMan>
    {
    }
}
