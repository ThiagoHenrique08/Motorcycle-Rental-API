using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Motorcycle_Rental_API;
using Motorcycle_Rental_Tests.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Motorcycle_Rental_Tests.Integration.MotorcycleIntegrationTest
{
    public sealed class DeleteMotorcycleIntegrationTests : BaseIntegrationTests, IDisposable
    {
        private HttpResponseMessage? _result;

        public DeleteMotorcycleIntegrationTests(CustomWebApplicationFactory<Program> factory)
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

            _result = await GetHttpClient().DeleteAsync($"/Motorcycles/{motorcycle.Identifier}");
            _result?.StatusCode.Should().Be(HttpStatusCode.OK);

            var deletedMotorcycle = _dbContext.Motorcycles.FirstOrDefault(c => c.Identifier == motorcycle.Identifier);
            deletedMotorcycle.Should().BeNull();
        }

        public new void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }

}
