using equilog_backend.Endpoints;

namespace equilog_backend.Startup;

public class AppInitialization
{
    public static void InitializeDevEnvironment(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    public static void InitializeHttpRedirection(WebApplication app)
    {
        app.UseHttpsRedirection();
    }
    
    public static void InitializeCors(WebApplication app)
    {
        app.UseCors("Default");
    }

    public static void InitializeAuthorization(WebApplication app)
    {
        app.UseAuthorization();
    }

    public static void InitializeEndpoints(WebApplication app)
    {
        CalendarEventEndpoints.RegisterEndpoints(app);
        HorseEndpoints.RegisterEndpoints(app);
        UserEndpoints.RegisterEndpoints(app);
        StableEnpoints.RegisterEndpoints(app);
    }
}