using FluentResults;
using Microsoft.Extensions.Logging;
using Motorcycle_Rental_Application.DTOs.DeliveryManDTO;
using Motorcycle_Rental_Application.Interfaces.DeliveryManInterfaces;
using Motorcycle_Rental_Application.Validators.DeliveryManValidators;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Application.UseCases.DeliveryManUseCase
{
    public class CreateDeliveryManUseCase(
        IDeliveryManRepository repository,
        ILogger<CreateDeliveryManUseCase> logger) : ICreateDeliveryManUseCase
    {
        private readonly IDeliveryManRepository _repository = repository;
        private readonly ILogger _logger = logger;

        public async Task<Result<DeliveryMan>> ExecuteAsync(CreateDeliveryManDTO dto)
        {
            var validationResult = new CreateDeliveryManDTOValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return Result.Fail<DeliveryMan>(errors);
            }

            if (await _repository.ExistsByCNPJAsync(dto.CNPJ))
                return Result.Fail<DeliveryMan>("CNPJ já cadastrado");

            if (await _repository.ExistsByCNHAsync(dto.CNHNumber))
                return Result.Fail<DeliveryMan>("CNH já cadastrada");

            var deliveryMan = new DeliveryMan
            {
                Identifier = dto.Identifier,
                Name = dto.Name,
                CNPJ = dto.CNPJ,
                BirthDate = dto.BirthDate,
                CNHNumber = dto.CNHNumber,
                CNHType = dto.CNHType,
                CNHImage = dto.CNHImage
                
            };

            await _repository.RegisterAsync(deliveryMan);

            return Result.Ok(deliveryMan);
        }
    }
}
