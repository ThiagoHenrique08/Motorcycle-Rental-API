using FluentResults;
using Microsoft.Extensions.Logging;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;
using Motorcycle_Rental_Application.Interfaces.MotorcycleInterfaces;
using Motorcycle_Rental_Application.Validators.MotorcycleValidators;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Application.UseCases.Motorcycle
{
    public class DeleteMotorcycleUseCase(IMotorcycleRepository motorcycleRepository, ILogger<DeleteMotorcycleUseCase> logger, ILocationRepository locationRepository) : IDeleteMotorcycleUseCase
    {
        private readonly ILogger _logger = logger;
        private readonly IMotorcycleRepository _motorcycleRepository = motorcycleRepository;
        private readonly ILocationRepository _locationRepository = locationRepository;

        public async Task<Result> ExecuteAsync(DeleteMotorcycleDTO request)
        {
            var validationResult = new DeleteMotorcycleDTOValidator().Validate(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    _logger.LogError("[ERR] DeleteMotorcycleUseCase: {error}", error);
                }
                return Result.Fail(errors);
            }

            var motorcycleResult = await _motorcycleRepository.RecoverByAsync(m=>m.Identifier.Equals(request.Id));

            if (motorcycleResult is null)
            {
                return Result.Fail("Motorcycle not found");
            }

            var locationVerify = await _locationRepository.RecoverByAsync(l=>l.Motorcycle_Id.Equals(motorcycleResult.Identifier));
            
            if (locationVerify is not null)
            {
                return Result.Fail("The motorcycle is currently leased and cannot be deleted.");
            }
            await _motorcycleRepository.DeleteAsync(motorcycleResult);



            return Result.Ok();
        }
    }
}
