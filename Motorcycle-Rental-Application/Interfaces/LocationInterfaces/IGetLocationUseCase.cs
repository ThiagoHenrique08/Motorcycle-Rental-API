using Motorcycle_Rental_Application.DTOs.LocationDTO;
using Motorcycle_Rental_Application.UseCases.LocationUseCase;
using Motorcycle_Rental_Domain.Models;

namespace Motorcycle_Rental_Application.Interfaces.LocationInterfaces
{
    public interface IGetLocationUseCase : IUseCase<string,  GetLocationDTO>
    {
        
    }

}
