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
        // Validate reset code.
        app.MapPost("/api/validate-reset-code", ValidateResetCode)
            .WithName("ValidateResetCode");
        
        // Reset password
        app.MapPost("/api/reset-password", ResetPassword)
            .WithName("RestPassword");
        
        // -- Endpoints for composed services --
        
        // Send password reset email.
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
    
    // -- Result generators for composed endpoints --
    private static async Task<IResult> SendPasswordResetEmail(IPasswordResetComposition passwordResetComposition,
        PasswordResetRequestCreateDto passwordResetRequestCreateDto)
    {
        return Result.Generate(await passwordResetComposition.SendPasswordResetEmailAsync(passwordResetRequestCreateDto.Email));
    }
}