using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Motorcycle_Rental_API;
using Motorcycle_Rental_Application.DTOs.LocationDTO;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Tests.Builder;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Motorcycle_Rental_Tests.Integration.LocationFeature
{
    public sealed class GetLocationByIdFeatureTest(CustomWebApplicationFactory<Program> factory)
        : BaseIntegrationTests(factory, new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                       .Build()), IDisposable
    {
        private Location? _locationData;
        private HttpResponseMessage? _result;

        [Fact]
        public async Task Test()
        {
            // Arrange: cria uma locação de teste

            // Token básico (sem roles)
            SetUserTokenInHeaders();

            // Token com role ADMIN
            SetUserTokenInHeaders(new[] { "ADMIN" });

            // Token com múltiplas roles
            SetUserTokenInHeaders(new[] { "ADMIN", "ENTREGADOR" });

            var deliveryMan = new DeliveryManBuilder().Build();
            var motorcycle = new MotorcycleBuilder().Build();
            _dbContext.DeliveryMans.Add(deliveryMan);
            _dbContext.Motorcycles.Add(motorcycle);
            await _dbContext.SaveChangesAsync();

            _locationData = new LocationBuilder()
                .WithEntregador_Id(deliveryMan.Identifier)
                .WithMotorcycle_Id(motorcycle.Identifier)
                .Build();

            var dbContext = GetApplicationDbContext();
            await dbContext.Locations.AddAsync(_locationData);
            await dbContext.SaveChangesAsync();

            // Act: faz a requisição GET
            _result = await GetHttpClient().GetAsync($"/Location/{_locationData.LocationId}");
            var responseContent = await _result.Content.ReadFromJsonAsync<GetLocationDTO>();

            // Assert: valida status e conteúdo
            _result.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().NotBeNull();
            responseContent!.LocationId.Should().Be(_locationData.LocationId);
            responseContent.DeliveryMan_Id.Should().Be(_locationData.DeliveryMan_Id);
            responseContent.Motorcycle_Id.Should().Be(_locationData.Motorcycle_Id);
            responseContent.StartDate.Should().BeCloseTo(_locationData.StartDate, TimeSpan.FromMilliseconds(100));
            responseContent.EndDate.Should().BeCloseTo(_locationData.EndDate, TimeSpan.FromMilliseconds(100));
        
                
        
        
        }



        public void Dispose()
        {
            var dbContext = GetApplicationDbContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }
    }
}
