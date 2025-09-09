using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Motorcycle_Rental_API.Utils;
using Motorcycle_Rental_Application.DTOs.LocationDTO;
using Motorcycle_Rental_Application.Interfaces.LocationInterfaces;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {


    
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateLocationDTO dto,
            [FromServices] ICreateLocationUseCase useCase,
            [FromServices] ILogger<LocationController> logger,
            [FromServices] IDeliveryManRepository deliveryMan,
            [FromServices] IMotorcycleRepository motorcycle)
        {
            return await EndpointUtils.CallUseCase(
                () => useCase.ExecuteAsync(dto, deliveryMan, motorcycle),
                logger,
                onSuccess: _ => StatusCode(201, new { mensagem = "Locação realizada com sucesso" }),
                onFailure: result => BadRequest(new { mensagem = "Erro ao cadastrar", errors = result.Errors })
            );
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            [FromRoute] string id,
            [FromServices]IGetLocationUseCase getLocationByIdUseCase,
            [FromServices] ILogger<LocationController> logger)
        {
            return await EndpointUtils.CallUseCase(async () =>
            {
                var result = await getLocationByIdUseCase.ExecuteAsync(id);
                return result.IsSuccess
                ? Result.Ok(result.Value)
                : Result.Fail<GetLocationDTO>(result.Errors);
            },
                logger,
                onSuccess: result => Ok(result.Value),
               onFailure: result =>
               {
                   if (result.Errors != null && result.Errors.Any())
                   {
                       if (result.Errors.Any(e => e.Message.Contains("não encontrada", StringComparison.OrdinalIgnoreCase)))
                       {
                           return NotFound(new { mensagem = "Locação não encontrada" });
                       }

                       return BadRequest(new { mensagem = "Dados Inválidos", errors = result.Errors });
                   }

                   return NotFound(new { mensagem = "Locação não encontrada" });
               }

             );

        }

        [HttpPut]
        [Route("{id}/return")]
        public async Task<IActionResult> Update(
            string id,
            [FromBody] UpdateLocationDTO dto,
            [FromServices] IUpdateLocationUseCase updateLocationUseCase,
            [FromServices] ILogger<LocationController> logger)
        {


            return await EndpointUtils.CallUseCase(
                () => updateLocationUseCase.ExecuteAsync(dto, id),
                logger,
                onSuccess: result => Ok(new { mensagem = "Data da devolução informada com sucesso" }),
                onFailure: result => BadRequest(new { mensagem = "Dados inválidos", errors = result.Errors })
            );
        }

    }
}

