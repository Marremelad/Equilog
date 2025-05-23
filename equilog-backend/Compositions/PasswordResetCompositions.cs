using System.Net;
using equilog_backend.Common;
using equilog_backend.DTOs.EmailSendDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Security;

namespace equilog_backend.Compositions;

public class PasswordResetCompositions(
    IPasswordService passwordService,
    IEmailService emailService,
    PasswordResetSettings passwordResetSettings) 
    : IPasswordResetComposition
{
    public async Task<ApiResponse<Unit>> SendPasswordResetEmailAsync(string email)
    {
        var passwordResetResponse = await passwordService.CreatePasswordResetRequestAsync(email);

        if (!passwordResetResponse.IsSuccess)
        {
            return ApiResponse<Unit>.Failure(passwordResetResponse.StatusCode,
                $"Failed to create password reset request: {passwordResetResponse.Message}");
        }

        var emailResponse = await emailService.SendEmailAsync(
            new EmailSendPasswordResetDto(passwordResetResponse.Value, passwordResetSettings.BaseUrl),
            email);

        if (!emailResponse.IsSuccess)
        {
            await passwordService.DeletePasswordResetRequestAsync(passwordResetResponse.Value!.Id);
            emailResponse.Message =
                $"Failed to send Email: {emailResponse.Message}. Password reset request creation was rolled back.";
            return emailResponse;
        }

        return ApiResponse<Unit>.Success(HttpStatusCode.OK,
            Unit.Value,
            "Email sent successfully");
    }
}