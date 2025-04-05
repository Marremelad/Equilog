using equilog_backend.Startup;

namespace equilog_backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;

        AppConfiguration.AddDocumentationServices(services);
        AppConfiguration.AddLibraryServices(services);
        AppConfiguration.AddFrameworkServices(services);
        AppConfiguration.AddPersistenceServices(services, builder);
        AppConfiguration.AddDomainServices(services);
        AppConfiguration.AddCorsServices(services);
        
        var app = builder.Build();

        AppInitialization.InitializeDevEnvironment(app);
        AppInitialization.InitializeHttpRedirection(app);
        AppInitialization.InitializeCors(app);
        AppInitialization.InitializeAuthorization(app);
        AppInitialization.InitializeEndpoints(app);
        
        app.Run();
    }
}