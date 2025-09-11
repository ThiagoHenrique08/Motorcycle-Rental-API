using Bogus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Motorcycle_Rental_API;
using Motorcycle_Rental_Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Motorcycle_Rental_Tests.Integration
{
    public class BaseIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        protected readonly HttpClient _httpClient;
        protected readonly ApplicationDbContext _dbContext;
        protected readonly IDeliveryManRepository _deliveryManRepository;
        protected readonly ILocationRepository _locationRepository;
        protected readonly IMotorcycleRepository _motorcycleRepository;
        protected readonly IConfiguration _configuration;
        public BaseIntegrationTests(CustomWebApplicationFactory<Program> factory, IConfiguration configuration)
        {
            _httpClient = factory.CreateDefaultClient();
            var scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _deliveryManRepository = scope.ServiceProvider.GetService<IDeliveryManRepository>();
            _locationRepository = scope.ServiceProvider.GetService<ILocationRepository>();
            _motorcycleRepository = scope.ServiceProvider.GetService<IMotorcycleRepository>();
            _configuration = configuration;
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
        public void SetUserTokenInHeaders(IEnumerable<string>? roles = null)
        {
            var token = GenerateTestToken(roles);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private string GenerateTestToken(IEnumerable<string>? roles = null)
        {
            // Claims básicos do usuário de teste
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, "testuser"),
        new Claim(ClaimTypes.Email, "testuser@email.com"),
        new Claim("id", Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // Adiciona roles se fornecidas
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            // Pega a chave secreta do appsettings
            var secretKey = _configuration["JWT:SecretKey"]
                            ?? throw new InvalidOperationException("JWT SecretKey não configurada");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Cria o token usando o Issuer e Audience da configuração
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:TokenValidityInMinutes"] ?? "30")),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted(); // limpa o banco após cada teste
            _dbContext.Dispose();
        }



        

    }
}