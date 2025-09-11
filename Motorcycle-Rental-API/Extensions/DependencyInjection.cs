using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;
using Motorcycle_Rental_Infrastructure.Repository;
using Motorcycle_Rental_Infrastructure.Services;
using System.Text;

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
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IApplicationUserRoleRepository, ApplicationUserRoleRepository>();
            services.AddScoped<IUserApplicationRepository, UserApplicationRepository>();
            services.AddScoped<IApplicationRoleRepository, ApplicationRoleRepository>();

            return services;
        }

        public static IServiceCollection AddContextService(this IServiceCollection services)
        {
            //Build da configuração localmente
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //  tenta pegar a variável de ambiente DATABASE_URL
                var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

                if (string.IsNullOrEmpty(connectionString))
                {
                    //  se não tiver variável, usa a conexão local
                    connectionString = configuration["ConnectionStringsLocal:DefaultConnectionLocal"];
                }

                options.UseNpgsql(connectionString);
            });

            return services;
        }

        public static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddUserManager<UserManager<ApplicationUser>>()
              .AddRoleManager<RoleManager<ApplicationRole>>()
              .AddSignInManager<SignInManager<ApplicationUser>>()
              .AddDefaultTokenProviders();

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

        public static IServiceCollection AddTokenService(this IServiceCollection services)
        {

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

        public static IServiceCollection AddAuthenticationService(this IServiceCollection services, WebApplicationBuilder builder)
        {

            var secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentException("Invalid secret Key"); //Lança uma exceção se ele não conseguir obter a chave secreta do arquivo de configuração

            services.AddAuthentication(options =>
            {
                //Define que por padrão o sistema irá usar um esquema de Token JWT
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //Lança o Desafio: se alguem tentar acessar um recurso protegido sem passar o token ele irá solicitar as credenciar do usuário
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //Configuração do Token JWT
                options.SaveToken = true; //Indica se o token deve ser salvo
                options.RequireHttpsMetadata = false; //Indica se é preciso HTTPS para transmitir o Token - Em produção colocar true
                options.TokenValidationParameters = new TokenValidationParameters() // configuração dos parametros de autenticação do Token
                {
                    ValidateIssuer = true,//define as configuração de validação do Issuer - Emissor
                    ValidateAudience = true,//define as configuração de validação do Audience - Cliente
                    ValidateLifetime = true, //define a configuração de validação do Tempo de Vida do Token
                    ValidateIssuerSigningKey = true, // validar a chave de assinatura do emissor
                    ClockSkew = TimeSpan.Zero, // Ajustar o tempo para tratar delay entre o servidor de autenticação e servidor de aplicação
                    ValidAudience = builder.Configuration["JWT:ValidAudience"], //atribuir valor na variavel pegando o valor do arquivo de configuração
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],  //atribuir valor na variavel pegando o valor do arquivo de configuração
                    IssuerSigningKey = new SymmetricSecurityKey(
                                        Encoding.UTF8.GetBytes(secretKey)) // Gera uma chave simetrica com base da secretKey para assinar o token 
                };
            });

            return services;
        }

        public static IServiceCollection AddAuthorizationService(this IServiceCollection services)
        {

            services.AddAuthorization(options =>
            {
                //ADMIN, SQUADLEAD,TECHLEAD,PO,DEVELOPER,BILLING
                //RequireRole - Exige que o usuário tenha uma determinada Role/Perfil para acessar um recurso protegido
                const string Admin = "ADMIN";
                const string Entregador = "ENTREGADOR";

                options.AddPolicy(Entregador, policy => policy.RequireRole(Entregador));
   
                //RequireAssertion - Permite definir uma expressão lambda e com uma condição customizada para autorização
                options.AddPolicy(Admin, policy => policy.RequireAssertion(context =>
                    context.User.HasClaim(claim => claim.Type == "id") ||context.User.IsInRole(Admin)));

            });

            return services;
        }

        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Motorcycle-Rental", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Bearer JWT",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                                    {
                                        {
                                            new OpenApiSecurityScheme
                                            {
                                                Reference = new OpenApiReference
                                                {
                                                    Type = ReferenceType.SecurityScheme,
                                                    Id = "Bearer"
                                                }
                                            },
                                            new string[] {}
                                        }
                                    });
            });
            return services;
        }

    }
}
