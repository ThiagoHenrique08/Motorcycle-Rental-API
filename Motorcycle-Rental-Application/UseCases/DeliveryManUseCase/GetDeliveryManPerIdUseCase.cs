using FluentResults;
using Microsoft.Extensions.Logging;
using Motorcycle_Rental_Application.DTOs.DeliveryManDTO;
using Motorcycle_Rental_Application.Interfaces.DeliveryManInterfaces;
using Motorcycle_Rental_Application.Validators.DeliveryManValidators;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Application.UseCases.DeliveryManUseCase
{
    public class GetDeliveryManPerIdUseCase(IDeliveryManRepository deliveryManRepository, ILogger<GetDeliveryManPerIdUseCase> logger) : IGetDeliveryManPerIdUseCase
    {
        private readonly ILogger _logger = logger;
        private readonly IDeliveryManRepository _motorcycleRepository = deliveryManRepository;

        public async Task<Result<DeliveryMan?>> ExecuteAsync(GetDeliveryManPerIdDTO id)
        {
            var validationResult = new GetDeliveryManPerIdDTOValidator().Validate(id);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    _logger.LogError("[ERR] GetMotorcycleUseCase: {error}", error);
                }
                return Result.Fail(errors);
            }

            var deliveryMan = await _motorcycleRepository.RecoverByAsync(m => m.Identifier.Equals(id));

            if (deliveryMan is null)
                return Result.Fail<DeliveryMan?>("Entregador não encontrado");

            return Result.Ok(deliveryMan);
        }


    }
}
