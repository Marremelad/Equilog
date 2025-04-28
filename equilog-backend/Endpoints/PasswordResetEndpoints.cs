using equilog_backend.Common;
using equilog_backend.CompositionDTOs;
using equilog_backend.CompositionInterfaces;
using equilog_backend.DTOs.PasswordResetDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class PasswordResetEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/api/validate-reset-code", ValidateResetCode)
            .WithName("ValidateResetCode");

        app.MapPost("/api/reset-password", ResetPassword)
            .WithName("RestPassword");
        
        // Composed endpoints.
        app.MapPost("/api/password-reset-email/send", SendPasswordResetEmail)
            .WithName("SendPasswordResetEmail");
    }

    private static async Task<IResult> ValidateResetCode(IPasswordResetService passwordResetService,
        ValidateResetCodeDto validateResetCodeDto)
    {
        return Result.Generate(await passwordResetService.ValidateResetCodeAsync(validateResetCodeDto));
    }

    private static async Task<IResult> ResetPassword(IPasswordResetService passwordResetService,
        PasswordResetDto passwordResetDto)
    {
        return Result.Generate(await passwordResetService.ResetPasswordAsync(passwordResetDto));
    }
    
    // Composed result generators.
    private static async Task<IResult> SendPasswordResetEmail(IPasswordResetComposition passwordResetComposition,
        PasswordResetRequestCreateDto passwordResetRequestCreateDto)
    {
        return Result.Generate(await passwordResetComposition.SendPasswordResetEmailAsync(passwordResetRequestCreateDto.Email));
    }
}