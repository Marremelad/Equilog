using equilog_backend.Data;
using equilog_backend.Interfaces;
using equilog_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Startup;

public class AppConfiguration
{
    public static void AddDocumentationServices(IServiceCollection services)
    {
        services.AddSwaggerGen();
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

    public static void AddPersistenceServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<EquilogDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
    }

    public static void AddDomainServices(IServiceCollection services)
    {
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