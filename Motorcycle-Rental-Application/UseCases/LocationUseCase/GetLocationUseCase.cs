using FluentResults;
using Microsoft.Extensions.Logging;
using Motorcycle_Rental_Application.DTOs.LocationDTO;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;
using Motorcycle_Rental_Application.Interfaces.LocationInterfaces;
using Motorcycle_Rental_Application.UseCases.MotorcycleUseCase;
using Motorcycle_Rental_Application.Validators.LocationValidators;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Application.UseCases.LocationUseCase
{
    public class GetLocationUseCase(ILocationRepository locationRepository, ILogger<GetMotorcyclePerIdUseCase> logger) : IGetLocationUseCase
    {
        private readonly ILogger _logger = logger;
        private readonly ILocationRepository _locationRepository = locationRepository;

        public async Task<Result<GetLocationDTO>> ExecuteAsync(string id)
        {
            var validationResult = new GetLocationByIdDTOValidator().Validate(id);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    _logger.LogError("[ERR] GetMotorcycleUseCase: {error}", error);
                }
                return Result.Fail(errors);
            }

            var location = await _locationRepository.RecoverByAsync(m => m.LocationId == id);

            if (location is null)
                return Result.Fail<GetLocationDTO>("Locação não encontrada");

            var locationDTO = new GetLocationDTO
            {
                LocationId = location.LocationId,
                DailyValue = location.DailyValue,
                DeliveryMan_Id = location.DeliveryMan_Id,
                Motorcycle_Id = location.Motorcycle_Id,
                StartDate = location.StartDate,
                EndDate = location.EndDate,
                EstimatedEndDate = location.EstimatedEndDate,
                ReturnDate = location.ReturnDate,
            };

            return Result.Ok(locationDTO);
        }

        public Task<Result<Motorcycle_Rental_Domain.Models.Motorcycle>> ExecuteAsync(GetMotorcyclePerIdDTO request)
        {
            throw new NotImplementedException();
        }


    }
}