using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Motorcycle_Rental_API.Controllers;
using Motorcycle_Rental_Application.DTOs.LocationDTO;
using Motorcycle_Rental_Application.Interfaces.LocationInterfaces;

namespace Motorcycle_Rental_Tests.Unit.LocationUseCases
{
    public class UpdateLocationUseCaseTest
    {
        private readonly Mock<ILogger<LocationController>> _loggerMock;

        public UpdateLocationUseCaseTest()
        {
            _loggerMock = new Mock<ILogger<LocationController>>();
        }

        [Fact]
        public async Task Update_ShouldReturn200_WhenSuccess()
        {
            // Arrange
            var useCaseMock = new Mock<IUpdateLocationUseCase>();
            useCaseMock
                .Setup(x => x.ExecuteAsync(It.IsAny<UpdateLocationDTO>(), It.IsAny<string>()))
                .ReturnsAsync(Result.Ok());

            var controller = new LocationController(_loggerMock.Object);

            // Act
            var result = await controller.Update("1", new UpdateLocationDTO(DateTime.UtcNow), useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturn400_WhenFailure()
        {
            // Arrange
            var useCaseMock = new Mock<IUpdateLocationUseCase>();
            useCaseMock
                .Setup(x => x.ExecuteAsync(It.IsAny<UpdateLocationDTO>(), It.IsAny<string>()))
                .ReturnsAsync(Result.Fail(new List<string> { "Erro ao atualizar" }));

            var controller = new LocationController(_loggerMock.Object);

            // Act
            var result = await controller.Update("1", new UpdateLocationDTO(DateTime.UtcNow), useCaseMock.Object);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }
    }
}
