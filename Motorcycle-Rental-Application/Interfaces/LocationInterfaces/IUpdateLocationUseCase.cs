using FluentResults;
using Motorcycle_Rental_Application.DTOs.LocationDTO;

namespace Motorcycle_Rental_Application.Interfaces.LocationInterfaces
{
    public interface IUpdateLocationUseCase : IUseCase<UpdateLocationDTO>
    {
        Task<Result> ExecuteAsync(UpdateLocationDTO request, string id);
    }
}
