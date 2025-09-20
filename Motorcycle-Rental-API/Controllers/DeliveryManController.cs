using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Motorcycle_Rental_API.Utils;
using Motorcycle_Rental_Application.DTOs.DeliveryManDTO;
using Motorcycle_Rental_Application.Interfaces.DeliveryManInterfaces;

namespace Motorcycle_Rental_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeliveryManController : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = "ENTREGADOR")]
        public async Task<IActionResult> Create(
            [FromBody] CreateDeliveryManDTO dto,
            [FromServices] ICreateDeliveryManUseCase useCase,
            [FromServices] ILogger<DeliveryManController> logger)
        {
            return await EndpointUtils.CallUseCase(
                () => useCase.ExecuteAsync(dto),
                logger,
                onSuccess: _ => StatusCode(201, new { mensagem = "Entregador cadastrado com sucesso!" }),
                onFailure: result => BadRequest(new { mensagem = "Erro ao cadastrar", errors = result.Errors })
            );
        }

        [HttpPost("{id}/cnh")]
        [Authorize(Policy = "ENTREGADOR")]
        public async Task<IActionResult> UploadCNH(
            string id,
            [FromBody] UploadCNHDTO dto,
            [FromServices] IUploadCNHImageUseCase useCase,
            [FromServices] ILogger<DeliveryManController> logger)
        {
            if (string.IsNullOrEmpty(dto.Imagem_CNH))
                return BadRequest(new { mensagem = "Imagem é obrigatória" });

            return await EndpointUtils.CallUseCase(
                () => useCase.ExecuteAsync(id, dto.Imagem_CNH, $"{id}.png", "image/png"),
                logger,
                onSuccess: _ => Ok(new { mensagem = "CNH enviada com sucesso!" }),
                onFailure: result => BadRequest(new { mensagem = "Erro ao enviar a CNH", errors = result.Errors })
            );
        }
    }
}
