using equilog_backend.Common;
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
}