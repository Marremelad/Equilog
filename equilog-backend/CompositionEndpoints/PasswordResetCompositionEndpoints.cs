using equilog_backend.Common;
using equilog_backend.CompositionDTOs;
using equilog_backend.CompositionInterfaces;

namespace equilog_backend.CompositionEndpoints;

public class PasswordResetCompositionEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/api/password-reset-email/send", SendPasswordResetEmail)
            .WithName("SendPasswordResetEmail");
    }

    private static async Task<IResult> SendPasswordResetEmail(IPasswordResetCompositionService passwordResetCompositionService,
        PasswordResetRequestCreateDto passwordResetRequestCreateDto)
    {
        return Result.Generate(await passwordResetCompositionService.SendPasswordResetEmailAsync(passwordResetRequestCreateDto.Email));
    }
}