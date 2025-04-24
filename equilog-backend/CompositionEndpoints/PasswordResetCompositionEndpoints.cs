using equilog_backend.Common;
using equilog_backend.Interfaces.CompositionInterfaces;

namespace equilog_backend.CompositionEndpoints;

public class PasswordResetCompositionEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/api/password-reset-email/send", SendPasswordResetEmail)
            .WithName("SendPasswordResetEmail");
    }

    private static async Task<IResult> SendPasswordResetEmail(IPasswordResetCompositionService passwordResetCompositionService, 
        int id, string recipient)
    {
        return Result.Generate(await passwordResetCompositionService.SendPasswordResetEmailAsync(id, recipient));
    }
}