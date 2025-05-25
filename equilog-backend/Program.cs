using equilog_backend.Startup;

namespace equilog_backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        AppConfiguration.ConfigureServices(builder);

        var app = builder.Build();
        PipelineInitialization.InitializePipeline(app);

        app.Run();
    }
}