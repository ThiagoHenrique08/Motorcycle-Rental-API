using FluentAssertions;
using Motorcycle_Rental_API;
using Motorcycle_Rental_Application.DTOs.LocationDTO;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Tests.Builder;
using System.Net;
using System.Net.Http.Json;

namespace Motorcycle_Rental_Tests.Integration.LocationFeature
{
    public sealed class UpdateLocationReturnDateFeatureTest : BaseIntegrationTests, IDisposable
        
    {
        private Location? _locationData;
        private HttpResponseMessage? _result;

        public  UpdateLocationReturnDateFeatureTest(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Test()
        {
            // --- Arrange: Criar uma locação de teste ---

            // Arrange: criar dados dependentes
            var deliveryMan = new DeliveryManBuilder().Build();

            var motorcycle = new MotorcycleBuilder().Build();


            _dbContext.DeliveryMans.Add(deliveryMan);
            _dbContext.Motorcycles.Add(motorcycle);
            await _dbContext.SaveChangesAsync();


            _locationData = new LocationBuilder()
                .WithEntregador_Id(deliveryMan.Identifier)
                .WithMotorcycle_Id(motorcycle.Identifier)
                .WithReturnDate(DateTime.UtcNow) // inicial sem devolução
                .Build();

            var dbContext = GetApplicationDbContext();
            dbContext.Locations.Add(_locationData);
            await dbContext.SaveChangesAsync();

            // DTO para atualizar a ReturnDate
            var updateDto = new UpdateLocationDTO(DateTime.UtcNow)
            {
                ReturnDate = DateTime.UtcNow
            };

            // --- Act: Chamar endpoint PUT ---
            _result = await GetHttpClient().PutAsJsonAsync(
                $"/Location/{_locationData.LocationId}/return",
                updateDto
            );

            // --- Assert: StatusCode ---
            _result?.StatusCode.Should().Be(HttpStatusCode.OK);

            // --- Assert: Verificar atualização no banco ---
            var updatedLocation = dbContext.Locations
                .FirstOrDefault(l => l.LocationId == _locationData.LocationId);

            updatedLocation.Should().NotBeNull();
            updatedLocation?.ReturnDate.Should().BeCloseTo(updateDto.ReturnDate, TimeSpan.FromMilliseconds(300));

            // Limpeza
            dbContext.Locations.Remove(updatedLocation!);
            await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            var dbContext = GetApplicationDbContext();
            dbContext.Database.EnsureDeleted(); // limpa o banco de teste
            dbContext.Dispose();
        }
    }
}
