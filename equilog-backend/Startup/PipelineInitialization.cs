using equilog_backend.Endpoints;

namespace equilog_backend.Startup;

public static class PipelineSetup
{
    public static void SetupPipeline(WebApplication app)
    {
        // Environment-specific setup
        SetupEnvironment(app);
        
        // Security and communication
        SetupHttps(app);
        SetupCors(app);
        
        // Authentication and authorization
        SetupSecurity(app);
        
        // API endpoints
        RegisterEndpoints(app);
    }
    
    private static void SetupEnvironment(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    public static void SetupHttps(WebApplication app)
    {
        app.UseHttpsRedirection();
    }
    
    public static void SetupCors(WebApplication app)
    {
        app.UseCors("Default");
    }

    public static void SetupSecurity(WebApplication app)
    {
        app.UseAuthorization();
    }

    public static void RegisterEndpoints(WebApplication app)
    {
        UserEndpoints.RegisterEndpoints(app);
        HorseEndpoints.RegisterEndpoints(app);
        StableEnpoints.RegisterEndpoints(app);
        StablePostEndpoints.RegisterEndpoints(app);
        CalendarEventEndpoints.RegisterEndpoints(app);
    }
}