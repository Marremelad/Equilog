using equilog_backend.Data;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Security;
using equilog_backend.Services;
using equilog_backend.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace equilog_backend.Startup;

public static class AppConfiguration
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        // Core services.
        AddCoreServices(services);
        ConfigureDatabase(services, configuration);

        // Authentication and security.
        ConfigureAuthentication(services, configuration);

        // Cross-cutting concerns.
        AddAutoMapperProfiles(services);
        ConfigureCors(services);

        // Application-specific services.
        AddApplicationServices(services);

        // API documentation.
        ConfigureSwagger(services);
    }
    private static void AddCoreServices(IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddEndpointsApiExplorer();
    }

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EquilogDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
    }

    private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        // Configure JWT settings from configuration.
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddSingleton(provider => provider.GetRequiredService<IOptions<JwtSettings>>().Value);

        // Set up authentication schemes.
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtKey = configuration["JwtSettings:Key"]
                             ?? throw new InvalidOperationException("JWT Key is not configured");

                var issuer = configuration["JwtSettings:Issuer"]
                             ?? throw new InvalidOperationException("JWT Issuer is not configured");

                var audience = configuration["JwtSettings:Audience"]
                               ?? throw new InvalidOperationException("JWT Audience is not configured");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey))
                };
            });
    }

    private static void AddAutoMapperProfiles(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));
    }

    private static void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("Default",
                policy =>
                {
                    policy.WithOrigins("http://localhost:63343")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }

    private static void AddApplicationServices(IServiceCollection services)
    {
        // Authentication services.
        services.AddScoped<IAuthService, AuthService>();

        // Core domain services.
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHorseService, HorseService>();
        services.AddScoped<IStableService, StableService>();

        // Feature specific services.
        services.AddScoped<IStablePostService, StablePostService>();
        services.AddScoped<ICalendarEventService, CalendarEventService>();
        services.AddScoped<IWallPostService, WallPostService>();

        // Validators
        services.AddScoped<IValidator<HorseCreateDto>, HorseCreateDtoValidator>();
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // Basic Swagger document configuration.
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Equilog API",
                Version = "v1",
                Description = "API for the Equilog application"
            });

            // Add JWT authentication to Swagger UI.
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
            });
        });
    }
}