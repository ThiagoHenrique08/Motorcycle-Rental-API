using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Motorcycle_Rental_Application.Interfaces.DeliveryManInterfaces;
using Motorcycle_Rental_Application.Interfaces.LocationInterfaces;
using Motorcycle_Rental_Application.Interfaces.Motorcycle;
using Motorcycle_Rental_Application.Interfaces.MotorcycleInterfaces;
using Motorcycle_Rental_Application.Interfaces.Services;
using Motorcycle_Rental_Application.Services;
using Motorcycle_Rental_Application.UseCases.DeliveryManUseCase;
using Motorcycle_Rental_Application.UseCases.LocationUseCase;
using Motorcycle_Rental_Application.UseCases.Motorcycle;
using Motorcycle_Rental_Application.UseCases.MotorcycleUseCase;
using Motorcycle_Rental_Application.Validators.MotorcycleValidators;
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
            services.AddScoped<IUploadCNHImageUseCase, UploadCNHImageUseCase>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ICreateLocationUseCase, CreateLocationUseCase>();
            services.AddScoped<IGetLocationUseCase, GetLocationUseCase>();
            services.AddScoped<IUpdateLocationUseCase, UpdateLocationUseCase>();
            services.AddScoped<IServiceCalculateDailyValue, ServiceCalculateDailyValue>();

            return services;
        }

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

        public static IServiceCollection AddValidatorsService(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(CreateMotorcycleDTOValidator).Assembly);


            return services;
        }

        }
}
