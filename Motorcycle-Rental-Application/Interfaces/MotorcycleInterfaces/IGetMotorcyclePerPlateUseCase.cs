using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;

namespace Motorcycle_Rental_Application.Interfaces.MotorcycleInterfaces
{
    public interface IGetMotorcyclePerPlateUseCase : IUseCase<GetMotorcyclePerPlateDTO, Motorcycle_Rental_Domain.Models.Motorcycle>
    {
    }
}
