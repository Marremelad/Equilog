using equilog_backend.Startup;

namespace equilog_backend;

public class Program
{
    public static void Main(string[] args)
    {
        // Create a new web application builder with command line arguments.
        var builder = WebApplication.CreateBuilder(args);
        
        // Configure all services and dependencies through the AppConfiguration class.
        AppConfiguration.ConfigureServices(builder);

        // Build the web application with all configured services.
        var app = builder.Build();
        
        // Initialize the request processing pipeline and register all endpoints.
        PipelineInitialization.InitializePipeline(app);

        // Start the application and begin listening for HTTP requests.
        app.Run();
    }
}