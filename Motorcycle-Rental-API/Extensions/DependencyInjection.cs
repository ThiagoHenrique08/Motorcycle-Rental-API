using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Motorcycle_Rental_Application.Interfaces.DeliveryManInterfaces;
using Motorcycle_Rental_Application.Interfaces.Motorcycle;
using Motorcycle_Rental_Application.Interfaces.MotorcycleInterface;
using Motorcycle_Rental_Application.Interfaces.MotorcycleInterfaces;
using Motorcycle_Rental_Application.UseCases.DeliveryManUseCase;
using Motorcycle_Rental_Application.UseCases.Motorcycle;
using Motorcycle_Rental_Application.UseCases.MotorcycleUseCase;
using Motorcycle_Rental_Infrastructure.Interfaces;
using Motorcycle_Rental_Infrastructure.Repository;
using Motorcycle_Rental_Infrastructure.Services;

namespace Motorcycle_Rental_API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCorsService(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                        policy =>
                        {
                            policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithExposedHeaders("X-Pagination");
                        });
            });

            return services;
        }

        public static IServiceCollection AddDomainService(this IServiceCollection services)
        {

            services.AddScoped<IConsumer, MotorcycleCreatedConsumer>();
            services.AddScoped<IMotorcycleRepository, MotorcylceRepository>();
            services.AddScoped<IMotorcycleNotificationRepository, MotorcycleNotificationRepository>();
            services.AddScoped<ICreateMotorcycleUseCase, CreateMotorcycleUseCase>();
            services.AddScoped<IDeleteMotorcycleUseCase, DeleteMotorcycleUseCase>();
            services.AddScoped<IGetMotorcyclePerIdUseCase, GetMotorcyclePerIdUseCase>();
            services.AddScoped<IGetMotorcyclePerPlateUseCase, GetMotorcyclePerPlateUseCase>();
            services.AddScoped<IUpdateMotorcycleUseCase, UpdateMotorcycleUseCase>();
            services.AddScoped<IDeliveryManRepository, DeliveryManRepository>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<ICreateDeliveryManUseCase, CreateDeliveryManUseCase>();
            services.AddScoped<IGetDeliveryManPerIdUseCase, GetDeliveryManPerIdUseCase>();
            services.AddScoped<IUploadCNHImageUseCase, UploadCNHImageUseCase>();



            return services;
        }

        //public static IServiceCollection AddContextService(this IServiceCollection services)
        //{
        //    var configuration = new ConfigurationBuilder()
        //       .AddJsonFile("appsettings.json")
        //       .Build();

        //    services.AddDbContext<ApplicationDbContext>(options =>
        //    {
        //        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));


        //    }, ServiceLifetime.Scoped);


        //    return services;
        //}
        public static IServiceCollection AddContextService(this IServiceCollection services)
        {
            // Build da configuração localmente
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // tenta pegar a variável de ambiente DATABASE_URL
                var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

                if (string.IsNullOrEmpty(connectionString))
                {
                    // se não tiver variável, usa a conexão local
                    connectionString = configuration["ConnectionStringsLocal:DefaultConnectionLocal"];
                }

                options.UseNpgsql(connectionString);
            });

            return services;
        }




        public static IServiceCollection AddRabbitMqService(this IServiceCollection services)
        {

            // MassTransit + RabbitMQ
            services.AddMassTransit(x =>
            {
                x.AddConsumer<MotorcycleCreatedConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host("rabbitmq", "/", host =>
                    {
                        host.Username("admin");
                        host.Password("Criativos123@");
                    });

                    config.ConfigureEndpoints(context);
                });
            });



            return services;
        }

    }
}
