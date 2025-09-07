using FluentResults;
using Motorcycle_Rental_Application.DTOs.LocationDTO;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Application.Interfaces.LocationInterfaces
{
    public interface ICreateLocationUseCase : IUseCase<CreateLocationDTO, Location>
    {
       public Task<Result<Location>> ExecuteAsync(CreateLocationDTO locationDTO, IDeliveryManRepository deliveryMan, IMotorcycleRepository motorcycle);
    }
}
