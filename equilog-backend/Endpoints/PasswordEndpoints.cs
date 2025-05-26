using equilog_backend.Common;
using equilog_backend.DTOs.PasswordDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class PasswordEndpoints
{
	public static void RegisterEndpoints(WebApplication app)
	{
		// Reset password. Does not require authorization since the operation will take place outside the application.
		app.MapPost("/api/reset-password", ResetPassword) // "/api/password-resets"
			.AddEndpointFilter<ValidationFilter<PasswordResetDto>>()
			.WithName("RestPassword");

		// Change password.
		app.MapPost("/api/change-password", ChangePassword) // "/api/users/password"
			.RequireAuthorization() // Operation takes place inside the application.
			.AddEndpointFilter<ValidationFilter<PasswordChangeDto>>()
			.WithName("ChangePassword");
	}

	private static async Task<IResult> ResetPassword(
		IPasswordService passwordService,
		PasswordResetDto passwordResetDto)
	{
		return Result.Generate(await passwordService.ResetPasswordAsync(passwordResetDto));
	}

	private static async Task<IResult> ChangePassword(
		IPasswordService passwordService,
		PasswordChangeDto passwordChangeDto)
	{
		return Result.Generate(await passwordService.ChangePasswordAsync(passwordChangeDto));
	}
}