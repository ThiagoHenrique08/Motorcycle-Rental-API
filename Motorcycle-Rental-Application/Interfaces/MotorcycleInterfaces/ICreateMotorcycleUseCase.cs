
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;

namespace Motorcycle_Rental_Application.Interfaces.MotorcycleInterface
{
    public interface  ICreateMotorcycleUseCase : IUseCase<CreateMotorcycleDTO, Motorcycle_Rental_Domain.Models.Motorcycle>
    {
    }
}
