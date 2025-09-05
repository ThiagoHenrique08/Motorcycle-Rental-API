using FluentResults;
using Microsoft.Extensions.Logging;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;
using Motorcycle_Rental_Application.Interfaces.MotorcycleInterface;
using Motorcycle_Rental_Application.Validators.MotorcycleValidators;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Application.UseCases.Motorcycle
{
    public class CreateMotorcycleUseCase(IMotorcycleRepository contactRepository, ILogger<CreateMotorcycleUseCase> logger) : ICreateMotorcycleUseCase
    {
        private readonly ILogger _logger = logger;
        private readonly IMotorcycleRepository _motorcycleRepository = contactRepository;


        public async Task<Result<Motorcycle_Rental_Domain.Models.Motorcycle>> ExecuteAsync(CreateMotorcycleDTO request)
        {

            var validationResult = new CreateMotorcycleDTOValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    _logger.LogError("[ERR] CreateContactUseCase: {error}", error);
                }
                return Result.Fail(errors);
            }

            var motorcycle = new Motorcycle_Rental_Domain.Models.Motorcycle
            {
                Identifier = request.Identifier,
                Model = request.Model,
                Year = request.Year,
                Plate = request.Plate
            };
            var existing = await _motorcycleRepository.RecoverByAsync(m => m.Plate == request.Plate);
            if (existing != null)
                return Result.Fail<Motorcycle_Rental_Domain.Models.Motorcycle>("Motorcycle with this plate already exists.");

            var contact = await _motorcycleRepository.RegisterAsync(motorcycle);

            return Result.Ok(contact);
        }
    }
}