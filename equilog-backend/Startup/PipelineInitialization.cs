using equilog_backend.Endpoints;

namespace equilog_backend.Startup;

// Static class that configures the ASP.NET Core request processing pipeline.
// Organizes middleware registration and endpoint configuration in a logical order for optimal request handling.
public static class PipelineInitialization
{
    // Initializes the complete request processing pipeline with all necessary middleware and endpoints.
    public static void InitializePipeline(WebApplication app)
    {
        // Environment-specific setup (development tools, logging, etc.).
        InitializeEnvironment(app);

        // Security and communication middleware.
        InitializeHttps(app);
        InitializeCors(app);

        // Authentication and authorization middleware.
        InitializeSecurity(app);

        // API endpoint registration and routing.
        RegisterEndpoints(app);
    }

    // Configures environment-specific middleware and tools.
    private static void InitializeEnvironment(WebApplication app)
    {
        // Enable Swagger API documentation and UI only in the development environment.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    // Configures HTTPS redirection for secure communication.
    private static void InitializeHttps(WebApplication app)
    {
        // Redirect all HTTP requests to HTTPS for security.
        app.UseHttpsRedirection();
    }

    // Configures Cross-Origin Resource Sharing (CORS) for client applications.
    private static void InitializeCors(WebApplication app)
    {
        // Apply the "Default" CORS policy configured in AppConfiguration.
        app.UseCors("Default");
    }

    // Configures authentication and authorization middleware.
    private static void InitializeSecurity(WebApplication app)
    {
        // Enable authorization checks for protected endpoints.
        app.UseAuthorization();
    }

    // Registers all API endpoints and their routing configurations.
    private static void RegisterEndpoints(WebApplication app)
    {
        // Authentication and security endpoints.
        AuthEndpoints.RegisterEndpoints(app);
        EmailEndpoints.RegisterEndpoints(app);
        PasswordEndpoints.RegisterEndpoints(app);
        MailTrapEndpoints.RegisterEndpoints(app);
        
        // Core entity management endpoints.
        UserEndpoints.RegisterEndpoints(app);
        UserStableEndpoints.RegisterEndpoints(app);
        HorseEndpoints.RegisterEndpoints(app);
        StableEndpoints.RegisterEndpoints(app);
        
        // Content and communication endpoints.
        StablePostEndpoints.RegisterEndpoints(app);
        CalendarEventEndpoints.RegisterEndpoints(app);
        CommentEndpoints.RegisterEndpoints(app);
        
        // Membership and invitation management endpoints.
        StableJoinRequestEndpoints.RegisterEndpoints(app);
        StableInviteEndpoints.RegisterEndpoints(app);
        
        // Relationship and utility endpoints.
        StableHorseEndpoints.RegisterEndpoints(app);
        StableLocationEndpoints.RegisterEndpoints(app);
        BlobStorageEndpoints.RegisterEndpoints(app);
    }
}