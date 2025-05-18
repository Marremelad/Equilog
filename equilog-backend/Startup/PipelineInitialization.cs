using equilog_backend.Endpoints;

namespace equilog_backend.Startup;

public static class PipelineInitialization
{
    public static void InitializePipeline(WebApplication app)
    {
        // Environment-specific setup.
        InitializeEnvironment(app);

        // Security and communication.
        InitializeHttps(app);
        InitializeCors(app);

        // Authentication and authorization.
        InitializeSecurity(app);

        // API endpoints.
        RegisterEndpoints(app);
    }

    private static void InitializeEnvironment(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    private static void InitializeHttps(WebApplication app)
    {
        app.UseHttpsRedirection();
    }

    private static void InitializeCors(WebApplication app)
    {
        app.UseCors("Default");
    }

    private static void InitializeSecurity(WebApplication app)
    {
        app.UseAuthorization();
    }

    private static void RegisterEndpoints(WebApplication app)
    {
        AuthEndpoints.RegisterEndpoints(app);
        EmailEndpoints.RegisterEndpoints(app);
        PasswordEndpoints.RegisterEndpoints(app);
        MailTrapEndpoints.RegisterEndpoints(app);
        UserEndpoints.RegisterEndpoints(app);
        UserStableEndpoints.RegisterEndpoints(app);
        HorseEndpoints.RegisterEndpoints(app);
        StableEndpoints.RegisterEndpoints(app);
        StablePostEndpoints.RegisterEndpoints(app);
        CalendarEventEndpoints.RegisterEndpoints(app);
        StableJoinRequestEndpoints.RegisterEndpoints(app);
        StableInviteEndpoints.RegisterEndpoints(app);
        StableHorseEndpoints.RegisterEndpoints(app);
        StableLocationEndpoints.RegisterEndpoints(app);
        CommentEndpoints.RegisterEndpoints(app);
    }
}