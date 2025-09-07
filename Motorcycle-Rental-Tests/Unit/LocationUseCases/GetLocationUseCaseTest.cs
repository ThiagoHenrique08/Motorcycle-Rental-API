using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Motorcycle_Rental_API.Controllers;
using Motorcycle_Rental_Application.DTOs.LocationDTO;
using Motorcycle_Rental_Application.Interfaces.LocationInterfaces;

namespace Motorcycle_Rental_Tests.Unit.LocationUseCases
{
    public class GetLocationUseCaseTest
    {
        private readonly Mock<ILogger<LocationController>> _loggerMock;

        public GetLocationUseCaseTest()
        {
            _loggerMock = new Mock<ILogger<LocationController>>();
        }

        [Fact]
        public async Task GetById_ShouldReturn200_WhenFound()
        {
            // Arrange
            var useCaseMock = new Mock<IGetLocationUseCase>();
            useCaseMock
                .Setup(x => x.ExecuteAsync(It.IsAny<string>()))
                .ReturnsAsync(Result.Ok(new GetLocationDTO()));

            var controller = new LocationController(_loggerMock.Object);

            // Act
            var result = await controller.GetById("1", useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetById_ShouldReturn400_WhenInvalid()
        {
            // Arrange
            var useCaseMock = new Mock<IGetLocationUseCase>();
            useCaseMock
                .Setup(x => x.ExecuteAsync(It.IsAny<string>()))
                .ReturnsAsync(Result.Fail<GetLocationDTO>(new List<string> { "Id inválido" }));

            var controller = new LocationController(_loggerMock.Object);

            // Act
            var result = await controller.GetById("1", useCaseMock.Object);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }


        [Fact]
        public async Task GetById_ShouldReturn404_WhenNotFound()
        {
            // Arrange
      
            var useCaseMock = new Mock<IGetLocationUseCase>();
            useCaseMock
                .Setup(x => x.ExecuteAsync(It.IsAny<string>()))
                .ReturnsAsync(Result.Fail<GetLocationDTO>("Locação não encontrada"));

            var controller = new LocationController(_loggerMock.Object);

            // Act
            var result = await controller.GetById("999", useCaseMock.Object);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }
    }
}
