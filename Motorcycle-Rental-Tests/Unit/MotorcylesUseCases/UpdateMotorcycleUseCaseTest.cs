using FluentResults;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Motorcycle_Rental_API.Controllers;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;
using Motorcycle_Rental_Application.Interfaces.Motorcycle;
using Motorcycle_Rental_Application.Interfaces.MotorcycleInterfaces;
using Motorcycle_Rental_Infrastructure.Interfaces;
using Motorcycle_Rental_Tests.Builder;
namespace Motorcycle_Rental_Tests.Unit.MotorcylesTests
{
    public class UpdateMotorcyclesUseCaseTest
    {
        private readonly Mock<ILogger<MotorcyclesController>> _loggerMock;
        private readonly MotorcyclesController _controller;

        public UpdateMotorcyclesUseCaseTest()
        {
            _loggerMock = new Mock<ILogger<MotorcyclesController>>();

            var fakeCreateUseCase = new Mock<ICreateMotorcycleUseCase>();
            var fakePublish = new Mock<IPublishEndpoint>();
            var fakeNotificationRepo = new Mock<IMotorcycleNotificationRepository>();
            
            _controller = new MotorcyclesController(

                fakeNotificationRepo.Object
             
            );
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenUseCaseSucceeds()
        {
            // Arrange
            var motorcycle = new MotorcycleBuilder().Build();

            var motorcycleDTO = new UpdateMotorcycleDTO { Plate = motorcycle.Plate };


            var _updateUseCaseMock = new Mock<IUpdateMotorcycleUseCase>();

            _updateUseCaseMock
                .Setup(u => u.ExecuteAsync(motorcycleDTO, motorcycle.Identifier))
                .ReturnsAsync(Result.Ok());

            // Act
            var result = await _controller.Update(motorcycle.Identifier, motorcycleDTO, _updateUseCaseMock.Object, _loggerMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

        


        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenUseCaseFails()
        {      // Arrange
            var motorcycle = new MotorcycleBuilder().Build();

            var motorcycleDTO = new UpdateMotorcycleDTO()
            {
                Plate = motorcycle.Plate + "PlacaErrada"

            };


            var _updateUseCaseMock = new Mock<IUpdateMotorcycleUseCase>();

            _updateUseCaseMock
                .Setup(u => u.ExecuteAsync(motorcycleDTO, motorcycle.Identifier))
                .ReturnsAsync(Result.Fail("Erro de validação"));

            // Act
            var result = await _controller.Update(motorcycle.Identifier, motorcycleDTO, _updateUseCaseMock.Object, _loggerMock.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

        }



    }
}
