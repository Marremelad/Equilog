using equilog_backend.Common;
using equilog_backend.DTOs.EmailDTOs;
using equilog_backend.DTOs.PasswordDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class PasswordEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Reset password.
        app.MapPost("/api/reset-password", ResetPassword)
            .WithName("RestPassword");
        
        // Change password.
        app.MapPost("/api/change-password", ChangePassword)
            .WithName("ChangePassword");
        
        // -- Endpoints for compositions --
        
        // Send password reset email.
        app.MapPost("/api/password-reset-email/send", SendPasswordResetEmail)
            .AddEndpointFilter<ValidationFilter<EmailDto>>()
            .WithName("SendPasswordResetEmail");
    }
    
    private static async Task<IResult> ResetPassword(IPasswordService passwordService,
        PasswordResetDto passwordResetDto)
    {
        return Result.Generate(await passwordService.ResetPasswordAsync(passwordResetDto));
    }

    private static async Task<IResult> ChangePassword(IPasswordService passwordService,
        PasswordChangeDto passwordChangeDto)
    {
        return Result.Generate(await passwordService.ChangePasswordAsync(passwordChangeDto));
    }
    
    // -- Result generators for compositions --
    private static async Task<IResult> SendPasswordResetEmail(IPasswordResetComposition passwordResetComposition, EmailDto emailDto)
    {
        return Result.Generate(await passwordResetComposition.SendPasswordResetEmailAsync(emailDto.Email));
    }
}