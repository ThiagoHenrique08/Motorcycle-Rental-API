using FluentResults;
using Microsoft.Extensions.Logging;
using Motorcycle_Rental_Application.DTOs.LocationDTO;
using Motorcycle_Rental_Application.Interfaces.LocationInterfaces;
using Motorcycle_Rental_Application.Interfaces.Services;
using Motorcycle_Rental_Application.Validators.LocationValidators;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Application.UseCases.LocationUseCase
{
    public class UpdateLocationUseCase(
        ILocationRepository locationRepository,
        ILogger<UpdateLocationUseCase> logger, IServiceCalculateDailyValue serviceCalculateDailyValue) : IUpdateLocationUseCase
    {

        private readonly ILocationRepository _locationRepository = locationRepository;
        private readonly ILogger<UpdateLocationUseCase> _logger = logger;
        private readonly IServiceCalculateDailyValue _serviceCalculateDailyValue = serviceCalculateDailyValue;

        public async Task<Result> ExecuteAsync(UpdateLocationDTO request, string id)
        {
            var validationResult = new UpdateLocationValidator().Validate(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                foreach (var error in errors)
                    _logger.LogError("[ERR] UpdateMotorcycleUseCase: {error}", error);

                return Result.Fail(errors);
            }
            // Recupera a entidade rastreada
            var location = await _locationRepository.RecoverByAsync(m => m.LocationId.Equals(id));

            if (location is null)
                return Result.Fail("Locação não encontrada");

            // Atualiza os campos
            location.ReturnDate = request.ReturnDate;
            location.DailyValue = await _serviceCalculateDailyValue.CalculatorDailyValue(request.ReturnDate,location.EstimatedEndDate, location.Plan);

            // Salva as alterações
            await _locationRepository.UpdateAsync(location);


            return Result.Ok();
        }

        public Task<Result> ExecuteAsync(UpdateLocationDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
