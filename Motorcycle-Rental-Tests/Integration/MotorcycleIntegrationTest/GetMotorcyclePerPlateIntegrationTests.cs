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

namespace Motorcycle_Rental_Tests.Integration.MotorcycleIntegrationTest
{
    public sealed class GetMotorcyclePerPlateIntegrationTests : BaseIntegrationTests, IDisposable
    {
        private HttpResponseMessage? _result;

        public GetMotorcyclePerPlateIntegrationTests(CustomWebApplicationFactory<Program> factory)
            : base(factory) { }

        [Fact]
        public async Task Test_GetMotorcyclePerPlate_ShouldReturn200AndEntity()
        {
            var motorcycle = new MotorcycleBuilder().Build();
            _dbContext.Motorcycles.Add(motorcycle);
            await _dbContext.SaveChangesAsync();

            _result = await GetHttpClient().GetAsync($"/Motorcycles/by-plate?plate={motorcycle.Plate}");
            _result?.StatusCode.Should().Be(HttpStatusCode.OK);

            var returnedMotorcycle = await _result.Content.ReadFromJsonAsync<Motorcycle>();
            returnedMotorcycle!.Plate.Should().Be(motorcycle.Plate);

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
