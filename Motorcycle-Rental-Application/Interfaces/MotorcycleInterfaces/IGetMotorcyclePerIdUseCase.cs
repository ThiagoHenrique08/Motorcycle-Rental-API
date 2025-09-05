using FluentResults;
using MassTransit;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;

namespace Motorcycle_Rental_Application.Interfaces.MotorcycleInterfaces
{
    public interface IGetMotorcyclePerIdUseCase : IUseCase<GetMotorcyclePerIdDTO, Motorcycle_Rental_Domain.Models.Motorcycle>
    {
        public Task<Result<Motorcycle_Rental_Domain.Models.Motorcycle>> ExecuteAsync(string id);
    }
}
