using FluentResults;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Motorcycle_Rental_API.Utils;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;
using Motorcycle_Rental_Application.Interfaces.Motorcycle;
using Motorcycle_Rental_Application.Interfaces.MotorcycleInterfaces;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MotorcyclesController : ControllerBase
    {
   
    
       
        public readonly IMotorcycleNotificationRepository _motorcycleNotification;
        public MotorcyclesController(IMotorcycleNotificationRepository motorcycleNotification)
        {
    
            _motorcycleNotification = motorcycleNotification;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMotorcycleDTO dto, 
            [FromServices] ICreateMotorcycleUseCase createMotorcycleUseCase,
            [FromServices] ILogger<MotorcyclesController> logger,
            [FromServices] IPublishEndpoint publishEndpoint)
        {
            return await EndpointUtils.CallUseCase(
                async () =>
                {
                    var result = await createMotorcycleUseCase.ExecuteAsync(dto);

                    if (!result.IsSuccess)
                        return result;

                    // Publica evento no RabbitMQ via MassTransit usando classe concreta
                    await publishEndpoint.Publish(new MotorcycleCreatedEvent(
                        result.Value.Identifier,
                        result.Value.Year,
                        result.Value.Model,
                        result.Value.Plate
                    ));

                    return result;
                },
                logger,
                onSuccess: result => StatusCode(201, new { mensagem = "Moto cadastrada com sucesso!" }),
                onFailure: result => BadRequest(new { mensagem = "Dados inválidos", errors = result.Errors })
            );
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            string id,
            [FromBody] UpdateMotorcycleDTO dto,
            [FromServices] IUpdateMotorcycleUseCase updateMotorcycleUseCase,
            [FromServices] ILogger<MotorcyclesController> logger)
        {


            return await EndpointUtils.CallUseCase(
                () => updateMotorcycleUseCase.ExecuteAsync(dto, id),
                logger,
                onSuccess: result => Ok(new { mensagem = "Placa modificada com sucesso" }),
                onFailure: result => BadRequest(new { mensagem = "Dados inválidos", errors = result.Errors })
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            string id,
            [FromServices] IDeleteMotorcycleUseCase deleteMotorcycleUseCase,
            [FromServices] ILogger<MotorcyclesController> logger)
        {
            return await EndpointUtils.CallUseCase(
                () => deleteMotorcycleUseCase.ExecuteAsync(new DeleteMotorcycleDTO(id)),
                logger,
                onSuccess: result => Ok(new { mensagem = "Motocicleta deletada com sucesso" }),
                onFailure: result => BadRequest(new { mensagem = "Dados inválidos", errors = result.Errors })
            );
        }

        [HttpGet("by-plate")]
        public async Task<IActionResult> GetPerPlate(
            [FromQuery] string plate,
            [FromServices] IGetMotorcyclePerPlateUseCase getMotorcyclePerPlateUseCase,
            [FromServices] ILogger<MotorcyclesController> logger)
        {
            return await EndpointUtils.CallUseCase(
                async () =>
                {
                    var result = await getMotorcyclePerPlateUseCase.ExecuteAsync(new GetMotorcyclePerPlateDTO(plate));
                    return result.IsSuccess
                        ? Result.Ok(result.Value)
                        : Result.Fail<Motorcycle_Rental_Domain.Models.Motorcycle>(result.Errors);
                },
                logger,
                onSuccess: result => Ok(result.Value),
                onFailure: result => NotFound(new { errors = result.Errors })
            );
        }

        [HttpGet("list")]
        public async Task<IEnumerable<MotorcycleNotification>> Get()
        {
            return await _motorcycleNotification.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerId(string id, 
            [FromServices] IGetMotorcyclePerIdUseCase getMotorcyclePerIdUseCase,
            [FromServices] ILogger<MotorcyclesController> logger)
        {
            return await EndpointUtils.CallUseCase(
                async () =>
                {
                    var result = await getMotorcyclePerIdUseCase.ExecuteAsync(id);
                    return result.IsSuccess
                    ? Result.Ok(result.Value)
                    : Result.Fail<Motorcycle_Rental_Domain.Models.Motorcycle>(result.Errors);
                },
                logger,
                onSuccess: result => Ok(result.Value),
                onFailure: result =>
                {
                    // Decide qual resposta dar dependendo do conteúdo de result.Errors ou do id
                    if (result.Errors != null && result.Errors.Any())
                    {
                        // Request mal formada
                        return BadRequest(new { mensagem = "Request mal formada", errors = result.Errors });
                    }
                    else
                    {
                        // Não encontrado
                        return NotFound(new { mensagem = "Moto não encontrada" });
                    }
                }
            );
        }
    }

}
