using FluentResults;
using Microsoft.Extensions.Logging;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;
using Motorcycle_Rental_Application.Interfaces.Motorcycle;
using Motorcycle_Rental_Application.Validators.MotorcycleValidators;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Application.UseCases.Motorcycle
{
    public class UpdateMotorcycleUseCase(IMotorcycleRepository motorcycleReposutory, ILogger<UpdateMotorcycleUseCase> logger) : IUpdateMotorcycleUseCase
    {
        private readonly ILogger _logger = logger;
        private readonly IMotorcycleRepository _motorcycleRepository = motorcycleReposutory;

        public async Task<Result> ExecuteAsync(UpdateMotorcycleDTO request, string id)
        {

            var validationResult = new UpdateMotorcycleDTOValidator().Validate(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                foreach (var error in errors)
                    _logger.LogError("[ERR] UpdateMotorcycleUseCase: {error}", error);

                return Result.Fail(errors);
            }

            // Recupera a entidade rastreada
            var motorcycle = await _motorcycleRepository.RecoverByAsync(m => m.Identifier.Equals(id));

            if (motorcycle is null)
                return Result.Fail("Moto não encontrada");

            // Atualiza os campos
            motorcycle.Plate = request.Plate ?? motorcycle.Plate;

            // Salva as alterações
            await _motorcycleRepository.UpdateAsync(motorcycle);

            return Result.Ok();
        }

        public Task<Result> ExecuteAsync(UpdateMotorcycleDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
