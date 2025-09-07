using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Motorcycle_Rental_API.Controllers;
using Motorcycle_Rental_Application.DTOs.DeliveryManDTO;
using Motorcycle_Rental_Application.Interfaces.DeliveryManInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Tests.Unit.DeliveryManUseCases
{
    public class UploadCNHImageUseCaseTest
    {
        [Fact]
        public async Task UploadCNH_ReturnsOk_WhenUseCaseSucceeds()
        {
            // Arrange
            
            var id = "entregador123";
            var dto = new UploadCNHDTO
            {
                Imagem_CNH = "base64string"
            };

            var useCaseMock = new Mock<IUploadCNHImageUseCase>();
            useCaseMock.Setup(u => u.ExecuteAsync(id, dto.Imagem_CNH, $"{id}.png", "image/png"))
                .ReturnsAsync(Result.Ok());

            var loggerMock = new Mock<ILogger<DeliveryManController>>();

            var controller = new DeliveryManController();

            // Act
            var result = await controller.UploadCNH(id, dto, useCaseMock.Object, loggerMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var value = okResult.Value as dynamic;
            Assert.NotNull(value);

        }

        [Fact]
        public async Task UploadCNH_ReturnsBadRequest_WhenUseCaseFails()
        {
            // Arrange
            var id = "entregador123";
            var dto = new UploadCNHDTO
            {
                Imagem_CNH = "base64string"
            };

            var useCaseMock = new Mock<IUploadCNHImageUseCase>();
            useCaseMock.Setup(u => u.ExecuteAsync(id, dto.Imagem_CNH, $"{id}.png", "image/png"))
                .ReturnsAsync(Result.Fail(new List<string> { "Erro ao processar imagem" }));

            var loggerMock = new Mock<ILogger<DeliveryManController>>();

            var controller = new DeliveryManController();

            // Act
            var result = await controller.UploadCNH(id, dto, useCaseMock.Object, loggerMock.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            var value = badRequestResult.Value as dynamic;
            Assert.NotNull(value);
            Assert.Contains("Erro ao enviar a CNH", badRequestResult.Value.ToString());

        }

        [Fact]
        public async Task UploadCNH_ReturnsBadRequest_WhenImageIsEmpty()
        {
            // Arrange
            var id = "entregador123";
            var dto = new UploadCNHDTO
            {
                Imagem_CNH = string.Empty
            };

            var useCaseMock = new Mock<IUploadCNHImageUseCase>();
            var loggerMock = new Mock<ILogger<DeliveryManController>>();
            var controller = new DeliveryManController();

            // Act
            var result = await controller.UploadCNH(id, dto, useCaseMock.Object, loggerMock.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            var value = badRequestResult.Value as dynamic;
            Assert.NotNull(value);
  
        }
    }
}
