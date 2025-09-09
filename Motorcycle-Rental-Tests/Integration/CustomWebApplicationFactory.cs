using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Motorcycle_Rental_Tests.Integration
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Configura o DbContext para o banco de teste
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                var testDbContextFactory = new TestDbContextFactory();
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(testDbContextFactory.ConnectionString);
                });

                // --- Aqui adicionamos o mock do RabbitMQ ---
                var publishEndpointMock = new Moq.Mock<IPublishEndpoint>();
                publishEndpointMock
                    .Setup(m => m.Publish(It.IsAny<object>(), default))
                    .Returns(Task.CompletedTask);

                // Remove implementação real se existir
                var publishDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IPublishEndpoint));
                if (publishDescriptor != null)
                    services.Remove(publishDescriptor);

                services.AddSingleton(publishEndpointMock.Object);
            });
        }
    }


}
