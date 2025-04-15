using System.Text;
using equilog_backend.Data;
using equilog_backend.Interfaces;
using equilog_backend.Security;
using equilog_backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace equilog_backend.Startup;

public class AppConfiguration
{
    public static void AddDocumentationServices(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Equilog API",
                Version = "v1",
                Description = "API for the Equilog application"
            });
        
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
    
    public static void AddLibraryServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));
    }

    public static void AddFrameworkServices(IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddEndpointsApiExplorer();
    }
    
    public static void AddAuthServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
        services.AddSingleton(provider => provider.GetRequiredService<IOptions<JwtSettings>>().Value);
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtKey = builder.Configuration["JwtSettings:Key"] 
                             ?? throw new InvalidOperationException("JWT Key is not configured");
                
                var issuer = builder.Configuration["JwtSettings:Issuer"]
                             ?? throw new InvalidOperationException("JWT Issuer is not configured");
                
                var audience = builder.Configuration["JwtSettings:Audience"]
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

    public static void AddPersistenceServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<EquilogDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
    }

    public static void AddDomainServices(IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        
        services.AddScoped<IUserService, UserService>();
        
        services.AddScoped<IHorseService, HorseService>();
        
        services.AddScoped<IStableService, StableService>();
        
        services.AddScoped<IStablePostService, StablePostService>();
        
        services.AddScoped<ICalendarEventService, CalendarEventService>();
    }
    
    public static void AddCorsServices(IServiceCollection services)
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
}