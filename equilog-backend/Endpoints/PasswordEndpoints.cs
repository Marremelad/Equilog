using equilog_backend.Common;
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
}