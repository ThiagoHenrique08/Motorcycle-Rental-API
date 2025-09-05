using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;
using Motorcycle_Rental_Application.UseCases.Motorcycle;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;
using Motorcycle_Rental_Tests.Builder;
using System.Linq.Expressions;
namespace Motorcycle_Rental_Tests.Unit.MotorcylesTests
{
    public class DeleteMotorcycleUseCaseTest
    {
        [Fact]
        public async Task ExecuteAsync_WhenContactIsDeletedSuccessfully_ShouldReturnSuccess()
        {
            // Arrange
            var mockRepository = new Mock<IMotorcycleRepository>();
            var mockLogger = new Mock<ILogger<DeleteMotorcycleUseCase>>();
    
            var identifier = "Moto123";
            var requestDto = new DeleteMotorcycleDTO(identifier);

            var motorcycle = new MotorcycleBuilder().WithIdentifier(identifier).Build();
            mockRepository
                .Setup(repo => repo.RecoverByAsync(It.IsAny<Expression<Func<Motorcycle, bool>>>()))
                .ReturnsAsync(motorcycle);
            mockRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<Motorcycle>()));

            var useCase = new DeleteMotorcycleUseCase(mockRepository.Object, mockLogger.Object);

            // Act
            var result = await useCase.ExecuteAsync(requestDto);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();

            mockRepository.Verify(repo => repo.DeleteAsync(motorcycle), Times.Once);
        }

    }
}
