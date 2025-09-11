using FluentAssertions;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;
using Motorcycle_Rental_Tests.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Motorcycle_Rental_API;
using Microsoft.Extensions.Configuration;

namespace Motorcycle_Rental_Tests.Integration.MotorcycleIntegrationTest
{
    public sealed class UpdateMotorcycleIntegrationTests : BaseIntegrationTests, IDisposable
    {
        private HttpResponseMessage? _result;

        public UpdateMotorcycleIntegrationTests(CustomWebApplicationFactory<Program> factory)
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

            var motorcycle = new MotorcycleBuilder().Build();
             await _motorcycleRepository.RegisterAsync(motorcycle);
           // await _dbContext.SaveChangesAsync();

            var updateDto = new UpdateMotorcycleDTO { Plate = "CDZ-1234" };
            
            _result = await GetHttpClient().PutAsJsonAsync($"/Motorcycles/{motorcycle.Identifier}", updateDto);
            _result?.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedMotorcycle = await _motorcycleRepository.RecoverByAsync(m => m.Identifier == motorcycle.Identifier);
            updatedMotorcycle.Should().NotBeNull();

            _dbContext.Motorcycles.Remove(updatedMotorcycle);
            await _dbContext.SaveChangesAsync();
        }

        public new void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }


}
