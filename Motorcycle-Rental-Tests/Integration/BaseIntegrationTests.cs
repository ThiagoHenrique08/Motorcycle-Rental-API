using Microsoft.Extensions.DependencyInjection;
using Motorcycle_Rental_API;
using Motorcycle_Rental_Infrastructure.Interfaces;

namespace Motorcycle_Rental_Tests.Integration
{
    public class BaseIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        protected readonly HttpClient _httpClient;
        protected readonly ApplicationDbContext _dbContext;
        protected readonly IDeliveryManRepository _deliveryManRepository;
        protected readonly ILocationRepository _locationRepository;
        protected readonly IMotorcycleRepository _motorcycleRepository;
        public BaseIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateDefaultClient();
            var scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _deliveryManRepository = scope.ServiceProvider.GetService<IDeliveryManRepository>();
            _locationRepository = scope.ServiceProvider.GetService<ILocationRepository>();
            _motorcycleRepository = scope.ServiceProvider.GetService<IMotorcycleRepository>();
        }

        public HttpClient GetHttpClient()
        {
            return _httpClient;
        }

        public ApplicationDbContext GetApplicationDbContext()
        {
            return _dbContext;
        }

        public IDeliveryManRepository GetDeliveryManRepository()
        {
            return _deliveryManRepository;
        }

        public ILocationRepository GetLocationRepository()
        {
            return _locationRepository;
        }

        public IMotorcycleRepository GetMotorcycleRepositoryy()
        {
            return _motorcycleRepository;
        }

        //private static string GenerateToken()
        //{
        //    var faker = new Faker("pt_BR");

        //    Claim[] claims =
        //    [
        //        new Claim("username", faker.Name.FullName()),
        //    new Claim("id", Guid.NewGuid().ToString()),
        //    new Claim("loginTimestamp", DateTime.UtcNow.ToString())
        //    ];

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c2d4a61141f0616bef9eac3c6cd539c454509dddfed9d0df54a6a17bfbe9172b"));

        //    var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        expires: DateTime.Now.AddMinutes(10),
        //        claims: claims,
        //        signingCredentials: signingCredentials
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted(); // limpa o banco após cada teste
            _dbContext.Dispose();
        }



        

    }
}