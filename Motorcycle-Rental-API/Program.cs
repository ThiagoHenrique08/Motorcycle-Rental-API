using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using MoreThanFollowUp.API.Extensions;
using Motorcycle_Rental_API.Extensions;
using Motorcycle_Rental_Application.Validators.MotorcycleValidators;

namespace Motorcycle_Rental_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCorsService();
            builder.Services.AddCorsService();
            builder.Services.AddDomainService();
            builder.Services.AddContextService();
            builder.Services.AddRabbitMqService();
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(ms => ms.Value.Errors.Count > 0)
                            .ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                            );

                        return new BadRequestObjectResult(new { errors });
                    };
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            // Habilitar FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();

            // Registrar todos os Validators da Assembly
            builder.Services.AddValidatorsFromAssemblyContaining<CreateMotorcycleDTOValidator>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Motorcycle API V1");
                c.RoutePrefix = string.Empty; // Swagger na raiz
            });
            app.ApplyMigrations();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddleware>();
            app.MapControllers();

            app.Run("http://0.0.0.0:8080");
        }
    }
}
