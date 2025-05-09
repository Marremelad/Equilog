using equilog_backend.Data;
using equilog_backend.Interfaces;
using equilog_backend.Security;
using equilog_backend.Services;
using equilog_backend.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SendGrid;
using System.Text;
using equilog_backend.Compositions;
using Twilio;

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
        ConfigurePasswordResetSettings(services, configuration);
        
        // Twilio and Sendgrid services.
        ConfigureTwilioSettings(services, configuration);
        ConfigureSendgridSettings(services, configuration);

        // Cross-cutting concerns.
        AddAutoMapperProfiles(services);
        ConfigureCors(services, configuration);

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
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        
        if (jwtSettings == null)
            throw new InvalidOperationException("JwtSettings not found in configuration");
        
        if (string.IsNullOrWhiteSpace(jwtSettings.Key))
            throw new InvalidOperationException("JwtSettings.Key not found in configuration");
        
        if (string.IsNullOrWhiteSpace(jwtSettings.Issuer))
            throw new InvalidOperationException("JwtSettings.Issuer not found in configuration");
        
        if (string.IsNullOrEmpty(jwtSettings.Audience))
           throw new InvalidOperationException("JwtSettings.Audience not found in configuration");
       
        services.AddSingleton(jwtSettings);
        
        // Set up authentication schemes.
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });
    }

    private static void ConfigurePasswordResetSettings(IServiceCollection services, IConfiguration configuration)
    {
        var passwordResetSettings = configuration.GetSection("PasswordResetSettings").Get<PasswordResetSettings>();

        if (passwordResetSettings == null)
            throw new InvalidOperationException("PasswordResetSettings not found in configuration");

        if (string.IsNullOrEmpty(passwordResetSettings.BaseUrl))
            throw new InvalidOperationException("PasswordResetSettings.BaseUrl not found in configuration");

        services.AddSingleton(passwordResetSettings);
    }
    
    private static void ConfigureTwilioSettings(IServiceCollection services, IConfiguration configuration)
    {
        var twilioSettings = configuration.GetSection("TwilioSettings").Get<TwilioSettings>();
    
        if (twilioSettings == null)
            throw new InvalidOperationException("TwilioSettings not found in configuration");
        
        if (string.IsNullOrWhiteSpace(twilioSettings.AccountSid))
            throw new InvalidOperationException("TwilioSettings.AccountSid not found in configuration");
        
        if (string.IsNullOrWhiteSpace(twilioSettings.AuthToken))
            throw new InvalidOperationException("TwilioSettings.AuthToken not found in configuration");
        
        if (string.IsNullOrWhiteSpace(twilioSettings.VerifySid))
            throw new InvalidOperationException("TwilioSettings.VerifySid not found in configuration");
        
        TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.AuthToken);
        
        services.AddSingleton(twilioSettings);
    }
    
    private static void ConfigureSendgridSettings(IServiceCollection services, IConfiguration configuration)
    {
        var sendGridSettings = configuration.GetSection("SendGridSettings").Get<SendGridSettings>();
        if (sendGridSettings == null)
            throw new InvalidOperationException("SendGridSettings not found in configuration");
        
        if (string.IsNullOrWhiteSpace(sendGridSettings.ApiKey))
            throw new InvalidOperationException("SendGridSettings.ApiKey not found in configuration");
        
        var client = new SendGridClient(sendGridSettings.ApiKey);
        services.AddSingleton(client);
    }

    private static void AddAutoMapperProfiles(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));
    }

    private static void ConfigureCors(IServiceCollection services, IConfiguration configuration)
    {
        var corsConnection = configuration["CorsConnection:Url"]
                     ?? throw new InvalidOperationException("Cors connection is not configured");
        
        services.AddCors(options =>
        {
            options.AddPolicy("Default",
                policy =>
                {
                    policy.WithOrigins(corsConnection)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }
    
    private static void AddApplicationServices(IServiceCollection services)
    {
        // Authentication services.
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordService, PasswordService>();
        
        // MailTrap services.
        services.AddScoped<IMailTrapService, MailTrapService>();

        // Core domain services.
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHorseService, HorseService>();
        services.AddScoped<IStableService, StableService>();

        // Relational services
        services.AddScoped<IUserStableService, UserStableService>();

        // Email service.
        services.AddScoped<IEmailService, EmailService>();

        // Feature specific services.
        services.AddScoped<IStablePostService, StablePostService>();
        services.AddScoped<ICalendarEventService, CalendarEventService>();
        services.AddScoped<IWallPostService, WallPostService>();
        services.AddScoped<IStableJoinRequestService, StableJoinRequestService>();
        services.AddScoped<IStableInviteService, StableInviteService>();
        
        // Composition services
        services.AddScoped<IStableComposition, StableComposition>();
        services.AddScoped<IPasswordResetComposition, PasswordResetComposition>();

        // Validators
        services.AddValidatorsFromAssemblyContaining<HorseCreateDtoValidator>();
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