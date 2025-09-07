using FluentResults;
using Microsoft.Extensions.Logging;
using Motorcycle_Rental_Application.DTOs.DeliveryManDTO;
using Motorcycle_Rental_Application.DTOs.LocationDTO;
using Motorcycle_Rental_Application.Interfaces.DeliveryManInterfaces;
using Motorcycle_Rental_Application.Interfaces.LocationInterfaces;
using Motorcycle_Rental_Application.Interfaces.Services;
using Motorcycle_Rental_Application.UseCases.DeliveryManUseCase;
using Motorcycle_Rental_Application.Validators.DeliveryManValidators;
using Motorcycle_Rental_Application.Validators.LocationValidators;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;
using Motorcycle_Rental_Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Application.UseCases.LocationUseCase
{
    
    public class CreateLocationUseCase(
        ILocationRepository repository,
        ILogger<CreateLocationUseCase> logger) : ICreateLocationUseCase
    {
       

        private readonly ILocationRepository _repository = repository;
        private readonly ILogger _logger = logger;
        private readonly IServiceCalculateDailyValue _serviceCalculatorDailyValue;

        public async Task<Result<Location>> ExecuteAsync(CreateLocationDTO dto, IDeliveryManRepository deliveryMan, IMotorcycleRepository motorcycle)
        {
            var validationResult = new CreateLocationDTOValidator().Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return Result.Fail<Location>(errors);
            }
            var deliveryManResult = await deliveryMan.RecoverByAsync(dm => dm.Identifier == dto.DeliveryMan_Id);

            if (deliveryManResult is null)
                return Result.Fail("DeliveryMan not found");
            
            if (deliveryManResult.CNHType != "A")
                return Result.Fail("Entregador não possui CNH Categoria A");

            var motorcycleResult = await motorcycle.RecoverByAsync(m => m.Identifier == dto.Motorcycle_Id);


            if (motorcycleResult is null)
                return Result.Fail("Motorcycle not found");


            var DailyValue = await _serviceCalculatorDailyValue.CalculatorDailyValue(null,dto.EstimatedEndDate, dto.Plan);

            var location = new Location
            {
                DeliveryMan_Id = deliveryManResult.Identifier,
                Motorcycle_Id = motorcycleResult.Identifier,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                EstimatedEndDate = dto.EstimatedEndDate,
                Plan = dto.Plan,
                DailyValue = DailyValue

            };

            await _repository.RegisterAsync(location);

            return Result.Ok(location);
        }

        public Task<Result<Location>> ExecuteAsync(CreateLocationDTO request)
        {
            throw new NotImplementedException();
        }

    }
}
