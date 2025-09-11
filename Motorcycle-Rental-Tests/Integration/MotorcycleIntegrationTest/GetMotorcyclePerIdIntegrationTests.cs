using FluentAssertions;
using Motorcycle_Rental_Domain.Models;
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
    public sealed class GetMotorcyclePerIdIntegrationTests : BaseIntegrationTests, IDisposable
    {
        private HttpResponseMessage? _result;

        public GetMotorcyclePerIdIntegrationTests(CustomWebApplicationFactory<Program> factory)
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
            _dbContext.Motorcycles.Add(motorcycle);
            await _dbContext.SaveChangesAsync();

            _result = await GetHttpClient().GetAsync($"/Motorcycles/{motorcycle.Identifier}");
            _result?.StatusCode.Should().Be(HttpStatusCode.OK);

            var returnedMotorcycle = await _result.Content.ReadFromJsonAsync<Motorcycle>();
            returnedMotorcycle!.Identifier.Should().Be(motorcycle.Identifier);

            _dbContext.Motorcycles.Remove(motorcycle);
            await _dbContext.SaveChangesAsync();
        }

        public new void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }

}
