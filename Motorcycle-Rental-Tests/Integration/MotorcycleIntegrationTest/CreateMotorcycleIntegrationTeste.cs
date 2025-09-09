using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Motorcycle_Rental_API;
using Motorcycle_Rental_API.Controllers;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Tests.Builder;
using Xunit;

namespace Motorcycle_Rental_Tests.Integration.MotorcycleIntegrationTest
{
    public sealed class CreateMotorcycleIntegrationTests : BaseIntegrationTests, IDisposable
    {
        private HttpResponseMessage? _result;

        public CreateMotorcycleIntegrationTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {

        }

        [Fact]
        public async Task Test()
        {
            // Arrange: cria DTO de teste
            var dto = new MotorcycleBuilder().Build();
            var createDto = new CreateMotorcycleDTO(dto.Identifier, dto.Year, dto.Model, dto.Plate);
 
            // Act: chama o endpoint
            _result = await GetHttpClient().PostAsJsonAsync("/Motorcycles", createDto);

            // Assert: status HTTP
            _result?.StatusCode.Should().Be(HttpStatusCode.Created);

            // Assert: verificar se a entidade realmente existe no banco
            var createdMotorcycle = _dbContext.Motorcycles.FirstOrDefault(c =>
                c.Identifier == dto.Identifier);

            createdMotorcycle.Should().NotBeNull();
            createdMotorcycle!.Year.Should().Be(dto.Year);
            createdMotorcycle!.Model.Should().Be(dto.Model);
            createdMotorcycle!.Plate.Should().Be(dto.Plate);

            // Cleanup: remove tudo do banco
            _dbContext.Motorcycles.Remove(createdMotorcycle);
            await _dbContext.SaveChangesAsync();
        }

        public new void Dispose()
        {
            _dbContext.Database.EnsureDeleted(); // limpa o banco de teste
            _dbContext.Dispose();
        }
    }
}
