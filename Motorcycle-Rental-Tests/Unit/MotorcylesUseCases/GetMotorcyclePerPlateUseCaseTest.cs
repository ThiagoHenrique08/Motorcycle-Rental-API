using FluentAssertions;
using FluentResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Moq;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;
using Motorcycle_Rental_Application.UseCases.Motorcycle;
using Motorcycle_Rental_Application.UseCases.MotorcycleUseCase;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;
using Motorcycle_Rental_Tests.Builder;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
namespace Motorcycle_Rental_Tests.Unit.MotorcylesTests
{
    public class GetMotorcyclePerPlateUseCaseTest
    {
        [Fact]
        public async Task ExecuteAsync_WhenRequestedPerPlate_ShouldReturnResultOk()
        {
            // Arrange
            var mockRepository = new Mock<IMotorcycleRepository>();
            var mockLogger = new Mock<ILogger<GetMotorcyclePerPlateUseCase>>();

            var request = new CreateMotorcycleDTOBuilder().Build();
            var expectedMotorcycle = new Motorcycle
            {
                Identifier = request.Identifier,
                Model = request.Model,
                Year = request.Year,
                Plate = request.Plate
            };

            var requestDto = new GetMotorcyclePerPlateDTO(expectedMotorcycle.Plate);

            mockRepository
                .Setup(repo => repo.RecoverByAsync(It.IsAny<Expression<Func<Motorcycle, bool>>>()))
                .ReturnsAsync(expectedMotorcycle);

            var useCase = new GetMotorcyclePerPlateUseCase(mockRepository.Object, mockLogger.Object);

            // Act
            var result = await useCase.ExecuteAsync(requestDto);



            // Assert

            Assert.NotNull(result);
            Assert.NotNull(result.ValueOrDefault);
            Assert.True(result.IsSuccess);
            Assert.IsType<Result<Motorcycle>>(result);

    
            // Verifica que chamou o repositório
            mockRepository.Verify(
                repo => repo.RecoverByAsync(It.IsAny<Expression<Func<Motorcycle, bool>>>()),
                Times.Once
            );
        }

        [Fact]
        public async Task ExecuteAsync_WhenPlateDoesNotExist_ShouldReturnResultFail()
        {
            // Arrange
            var mockRepository = new Mock<IMotorcycleRepository>();
            var mockLogger = new Mock<ILogger<GetMotorcyclePerPlateUseCase>>();

            // Placa válida para passar no validator
            var requestDto = new GetMotorcyclePerPlateDTO("ABC1234");

            // Configura o mock para retornar null (placa não encontrada)
            mockRepository
                .Setup(repo => repo.RecoverByAsync(It.IsAny<Expression<Func<Motorcycle, bool>>>()))
                .ReturnsAsync((Motorcycle?)null);

            var useCase = new GetMotorcyclePerPlateUseCase(mockRepository.Object, mockLogger.Object);

            // Act
            var result = await useCase.ExecuteAsync(requestDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess); // deve falhar
            Assert.Null(result.ValueOrDefault); // não encontrou moto
            Assert.IsType<Result<Motorcycle>>(result);

            // Verifica que o repositório foi chamado uma vez
            mockRepository.Verify(
                repo => repo.RecoverByAsync(It.IsAny<Expression<Func<Motorcycle, bool>>>()),
                Times.Once
            );
        }


    }
}
