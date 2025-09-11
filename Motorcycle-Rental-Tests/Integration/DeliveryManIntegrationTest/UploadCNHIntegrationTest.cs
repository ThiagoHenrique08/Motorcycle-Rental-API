using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Motorcycle_Rental_API;
using Motorcycle_Rental_Application.DTOs.DeliveryManDTO;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Tests.Builder;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Motorcycle_Rental_Tests.Integration.DeliveryManIntegrationTest
{
    public sealed class UploadCNHIntegrationTest : BaseIntegrationTests, IDisposable
    {

        private Location? _locationData;
        private HttpResponseMessage? _result;

        public UploadCNHIntegrationTest(CustomWebApplicationFactory<Program> factory)
            : base(factory, new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                       .Build()) { }


        [Fact]
        public async Task Test()
        {
            // Arrange

            // Token básico (sem roles)
            SetUserTokenInHeaders();

            // Token com role ADMIN
            SetUserTokenInHeaders(new[] { "ADMIN" });

            // Token com múltiplas roles
            SetUserTokenInHeaders(new[] { "ADMIN", "ENTREGADOR" });

            var deliveryMan = new DeliveryManBuilder().Build();
            _dbContext.DeliveryMans.Add(deliveryMan);
            await _dbContext.SaveChangesAsync();

            var id = deliveryMan.Identifier;
            var dto = new UploadCNHDTO
            {
                Imagem_CNH = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("fake-image-content"))
            };

            // Act
            _result = await GetHttpClient().PostAsJsonAsync($"/DeliveryMan/{id}/cnh", dto);

            // Assert
            _result?.StatusCode.Should().Be(HttpStatusCode.OK);


        }
        public new void Dispose()
        {
            _dbContext.Database.EnsureDeleted(); // limpa o banco de teste
            _dbContext.Dispose();
        }
    }
}

