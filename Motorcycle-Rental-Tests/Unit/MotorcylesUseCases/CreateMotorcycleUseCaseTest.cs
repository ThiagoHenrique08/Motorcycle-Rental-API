using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Motorcycle_Rental_Application.UseCases.Motorcycle;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;
using Motorcycle_Rental_Tests.Builder;
namespace Motorcycle_Rental_Tests.Unit.MotorcylesTests
{
    public class CreateMotorcycleUseCaseTest
    {
        [Fact]
        public async Task ExecuteAsync_WhenValidRequest_ShouldReturnOk()
        {
            // Arrange
            var mockRepository = new Mock<IMotorcycleRepository>();
            var mockLogger = new Mock<ILogger<CreateMotorcycleUseCase>>();
       

            var request = new CreateMotorcycleDTOBuilder().Build();
            var expectedMotorcycle = new Motorcycle
            {
                Identifier = request.Identifier,
                Model = request.Model,
                Year = request.Year,
                Plate = request.Plate
            };

            mockRepository
                .Setup(repo => repo.RegisterAsync(It.IsAny<Motorcycle>()))
                .ReturnsAsync(expectedMotorcycle);

            var useCase = new CreateMotorcycleUseCase(mockRepository.Object, mockLogger.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            result.Value.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();

            result.Value.Identifier.Should().Be(expectedMotorcycle.Identifier);
            result.Value.Model.Should().Be(expectedMotorcycle.Model);
            result.Value.Year.Should().Be(expectedMotorcycle.Year);
            result.Value.Plate.Should().Be(expectedMotorcycle.Plate);
     

            mockRepository.Verify(repo => repo.RegisterAsync(It.Is<Motorcycle>(c =>
                c.Identifier == request.Identifier &&
                c.Model == request.Model &&
                c.Year == request.Year &&
                c.Plate == request.Plate
            )), Times.Once());
        }

        [Fact]
        public async Task ExecuteAsync_WhenMotorcycleHasInvalidFields_ShouldReturnResultFail()
        {
            // Arrange
            var mockRepository = new Mock<IMotorcycleRepository>();
            var mockLogger = new Mock<ILogger<CreateMotorcycleUseCase>>();
      

            var request = new CreateMotorcycleDTOBuilder().WithIdentifier("").Build();
            var expectedContact = new Motorcycle
            {
                Identifier = request.Identifier,
                Model = request.Model,
                Year = request.Year,
                Plate = request.Plate
            };

            mockRepository
                .Setup(repo => repo.RegisterAsync(It.IsAny<Motorcycle>()))
                .ThrowsAsync(new Exception("Validation failed."));

            var useCase = new CreateMotorcycleUseCase(mockRepository.Object, mockLogger.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();

            mockRepository.Verify(repo => repo.RegisterAsync(It.IsAny<Motorcycle>()), Times.Never());
        }
    }
}
