using FluentResults;
using Microsoft.Extensions.Logging;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;
using Motorcycle_Rental_Application.Interfaces.MotorcycleInterfaces;
using Motorcycle_Rental_Application.Validators.MotorcycleValidators;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Application.UseCases.MotorcycleUseCase
{
    public class GetMotorcyclePerPlateUseCase(IMotorcycleRepository motorcycleRepository, ILogger<GetMotorcyclePerPlateUseCase> logger) : IGetMotorcyclePerPlateUseCase
    {
        private readonly ILogger _logger = logger;
        private readonly IMotorcycleRepository _motorcycleRepository = motorcycleRepository;

        public async Task<Result<Motorcycle_Rental_Domain.Models.Motorcycle?>> ExecuteAsync(GetMotorcyclePerPlateDTO request)
        {
            var validationResult = new GetMotorcyclePerPlateDTOValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    _logger.LogError("[ERR] GetMotorcycleUseCase: {error}", error);
                }
                return Result.Fail(errors);
            }

            var motorcycle = await _motorcycleRepository.RecoverByAsync(m => m.Plate.Equals(request.Plate));


            if (motorcycle is null)
            {
                return Result.Fail("Motorcycle not found");
            }

            return Result.Ok(motorcycle);
        }
    }
}