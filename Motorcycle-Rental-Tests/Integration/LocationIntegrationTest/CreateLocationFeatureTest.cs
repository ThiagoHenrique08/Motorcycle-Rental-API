using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Motorcycle_Rental_API;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Tests.Builder;
using System.Net;
using System.Net.Http.Json;

namespace Motorcycle_Rental_Tests.Integration.LocationIngrationTest;

public sealed class CreateLocationFeatureTest : BaseIntegrationTests, IDisposable
{
    private Location? _locationData;
    private HttpResponseMessage? _result;

    public CreateLocationFeatureTest(CustomWebApplicationFactory<Program> factory)
        : base(factory, new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                       .Build()) { }

    [Fact]
    public async Task Test()
    {
        // Token básico (sem roles)
        SetUserTokenInHeaders();

        // Token com role ADMIN
        SetUserTokenInHeaders(new[] { "ADMIN" });

        // Token com múltiplas roles
        SetUserTokenInHeaders(new[] { "ADMIN", "ENTREGADOR" });

        // Arrange: criar dados dependentes
        var deliveryMan = new DeliveryManBuilder().Build();

        var motorcycle = new MotorcycleBuilder().Build();


        _dbContext.DeliveryMans.Add(deliveryMan);
        _dbContext.Motorcycles.Add(motorcycle);
        await _dbContext.SaveChangesAsync();

        // Criar location referenciando esses dados
        _locationData = new LocationBuilder()
            .WithEntregador_Id(deliveryMan.Identifier)
            .WithMotorcycle_Id(motorcycle.Identifier)
            .Build();

        // Act: chama o endpoint
        _result = await GetHttpClient().PostAsJsonAsync("/Location", _locationData);

        // Assert: status do endpoint
        _result?.StatusCode.Should().Be(HttpStatusCode.Created);

        // Assert: verificar se a entidade realmente existe no banco
        var createdLocation = _dbContext.Locations.FirstOrDefault(c =>
            c.DeliveryMan_Id == _locationData.DeliveryMan_Id &&
            c.Motorcycle_Id == _locationData.Motorcycle_Id &&
            c.StartDate == _locationData.StartDate
        );

        createdLocation.Should().NotBeNull();
        createdLocation!.DailyValue.Should().Be(_locationData.DailyValue = 210);
        createdLocation!.StartDate.Should().BeCloseTo(_locationData.StartDate, TimeSpan.FromMilliseconds(1));
        createdLocation!.EndDate.Should().BeCloseTo(_locationData.EndDate, TimeSpan.FromMilliseconds(1));
        createdLocation.EstimatedEndDate.Should().BeCloseTo(_locationData.EstimatedEndDate, TimeSpan.FromMilliseconds(1));
        createdLocation.Plan.Should().Be(_locationData.Plan);

        // Cleanup: remove tudo do banco
        _dbContext.Locations.Remove(createdLocation);
        _dbContext.DeliveryMans.Remove(deliveryMan);
        _dbContext.Motorcycles.Remove(motorcycle);
        await _dbContext.SaveChangesAsync();
    }

    public new void Dispose()
    {
        _dbContext.Database.EnsureDeleted(); // limpa o banco de teste
        _dbContext.Dispose();
    }
}
