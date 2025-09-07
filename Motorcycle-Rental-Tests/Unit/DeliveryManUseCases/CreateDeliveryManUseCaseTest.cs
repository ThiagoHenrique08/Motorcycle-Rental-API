using FluentAssertions;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Motorcycle_Rental_API.Controllers;
using Motorcycle_Rental_Application.DTOs.DeliveryManDTO;
using Motorcycle_Rental_Application.Interfaces.DeliveryManInterfaces;
using Motorcycle_Rental_Tests.Builder;

namespace Motorcycle_Rental_Tests.Unit.DeliveryManUseCases
{
    public class CreateDeliveryManUseCaseTest
    {
        [Fact]
        public async Task Create_Returns201_WhenUseCaseSucceeds()
        {
            // Arrange

            var createBuilder = new CreateDeliveyManDTOBuilder();
            var dto = new CreateDeliveryManDTO
            {
                Identifier = createBuilder.Identifier,
                Name = createBuilder.Name,
                CNPJ = createBuilder.CNPJ,
                BirthDate = createBuilder.BirthDate,
                CNHNumber = createBuilder.CNHNumber,
                CNHType = createBuilder.CNHType,
                CNHImage = createBuilder.CNHImage
            };

            var useCaseMock = new Mock<ICreateDeliveryManUseCase>();
            
            useCaseMock.Setup(u => u.ExecuteAsync(dto))
                .ReturnsAsync(Result.Ok()); // Sucesso

            var loggerMock = new Mock<ILogger<DeliveryManController>>();

            var controller = new DeliveryManController();

            // Act
            var result = await controller.Create(dto, useCaseMock.Object, loggerMock.Object);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, objectResult.StatusCode);
            Assert.Contains("Entregador cadastrado com sucesso", objectResult.Value.ToString());
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenUseCaseFails()
        {
            // Arrange
            var createBuilder = new CreateDeliveyManDTOBuilder();
            var dto = new CreateDeliveryManDTO
            {
                Identifier = createBuilder.Identifier,
                Name = createBuilder.Name,
                CNPJ = createBuilder.CNPJ,
                BirthDate = createBuilder.BirthDate,
                CNHNumber = createBuilder.CNHNumber,
                CNHType = createBuilder.CNHType,
                CNHImage = createBuilder.CNHImage
            };

            var useCaseMock = new Mock<ICreateDeliveryManUseCase>();
            useCaseMock.Setup(u => u.ExecuteAsync(dto))
                .ReturnsAsync(Result.Fail(new List<string> { "Erro de validação" }));

            var loggerMock = new Mock<ILogger<DeliveryManController>>();

            var controller = new DeliveryManController();

            // Act
            var result = await controller.Create(dto, useCaseMock.Object, loggerMock.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            result.Should().NotBeNull();
        }
    }
}
