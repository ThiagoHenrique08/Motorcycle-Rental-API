using FluentResults;
using Microsoft.Extensions.Logging;
using Motorcycle_Rental_Application.Interfaces.DeliveryManInterfaces;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Application.UseCases.DeliveryManUseCase
{
    public class UploadCNHImageUseCase(
        IDeliveryManRepository repository,
        IStorageService storageService,
        ILogger<UploadCNHImageUseCase> logger) : IUploadCNHImageUseCase
    {
        private readonly IDeliveryManRepository _repository = repository;
        private readonly IStorageService _storageService = storageService;
        private readonly ILogger _logger = logger;

        public async Task<Result> ExecuteAsync(string deliveryManId, string base64Image, string fileName, string contentType)
        {
            var deliveryMan = await _repository.RecoverByAsync(d => d.Identifier == deliveryManId);
            if (deliveryMan == null)
                return Result.Fail("Entregador não encontrado");

            try
            {
                // Converter Base64 para bytes e salvar local
                var imageBytes = Convert.FromBase64String(base64Image);
                using var stream = new MemoryStream(imageBytes);
                await _storageService.SaveFileAsync(stream, fileName, contentType);

                // Salvar a Base64 no banco
                deliveryMan.CNHImage = base64Image;
                await _repository.UpdateAsync(deliveryMan);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dados inválidos");
                return Result.Fail("Dados inválidos");
            }
        }
    }
}