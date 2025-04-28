using equilog_backend.Common;
using equilog_backend.DTOs.EmailDTOs;
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
        
        // -- Endpoints for compositions --
        
        // Send password reset email.
        app.MapPost("/api/password-reset-email/send", SendPasswordResetEmail)
            .AddEndpointFilter<ValidationFilter<EmailDto>>()
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
    
    // -- Result generators for compositions --
    private static async Task<IResult> SendPasswordResetEmail(IPasswordResetComposition passwordResetComposition, EmailDto emailDto)
    {
        return Result.Generate(await passwordResetComposition.SendPasswordResetEmailAsync(emailDto.Email));
    }
}