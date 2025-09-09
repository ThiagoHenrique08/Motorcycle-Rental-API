using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Motorcycle_Rental_API.Controllers;
using Motorcycle_Rental_Application.DTOs.LocationDTO;
using Motorcycle_Rental_Application.Interfaces.LocationInterfaces;
using Motorcycle_Rental_Infrastructure.Interfaces;
using Motorcycle_Rental_Tests.Builder;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Motorcycle_Rental_Tests.Unit.LocationUseCases
{
    public class CreateLocationUseCaseTest
    {

        private readonly Mock<ILogger<LocationController>> _loggerMock;

        public CreateLocationUseCaseTest()
        {
            _loggerMock = new Mock<ILogger<LocationController>>();
        }


        [Fact]
        public async Task Create_ShouldReturn201_WhenSuccess()
        {
            // Arrange
            var createBuilder = new CreateLocationDTOBuilder();
            var useCaseMock = new Mock<ICreateLocationUseCase>();
            useCaseMock
                .Setup(x => x.ExecuteAsync(It.IsAny<CreateLocationDTO>(), It.IsAny<IDeliveryManRepository>(), It.IsAny<IMotorcycleRepository>()))
                .ReturnsAsync(Result.Ok());

            var controller = new LocationController();

            // Act
            var result = await controller.Create(new CreateLocationDTO(createBuilder.DeliveryMan_Id, createBuilder.Motorcycle_Id,
                createBuilder.StartDate, createBuilder.EndDate, createBuilder.EstimatedEndDate,
                createBuilder.Plan), useCaseMock.Object, _loggerMock.Object, Mock.Of<IDeliveryManRepository>(), Mock.Of<IMotorcycleRepository>());

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, statusResult.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldReturn400_WhenFailure()
        {
            // Arrange
            var createBuilder = new CreateLocationDTOBuilder();
            var useCaseMock = new Mock<ICreateLocationUseCase>();
            useCaseMock
                .Setup(x => x.ExecuteAsync(It.IsAny<CreateLocationDTO>(), It.IsAny<IDeliveryManRepository>(), It.IsAny<IMotorcycleRepository>()))
                .ReturnsAsync(Result.Fail(new List<string> { "Erro de validação" }));

            var controller = new LocationController();

            // Act
            var result = await controller.Create(new CreateLocationDTO(createBuilder.DeliveryMan_Id, createBuilder.Motorcycle_Id,
                createBuilder.StartDate, createBuilder.EndDate, createBuilder.EstimatedEndDate,
                createBuilder.Plan), useCaseMock.Object, _loggerMock.Object, Mock.Of<IDeliveryManRepository>(), Mock.Of<IMotorcycleRepository>());

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
        }



    }
}
