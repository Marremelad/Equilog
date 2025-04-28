using System.Net;
using equilog_backend.Common;
using equilog_backend.DTOs.EmailDTOs;
using equilog_backend.DTOs.EmailSendDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Compositions;

public class PasswordResetComposition(IPasswordResetService passwordResetService, IEmailService emailService) 
    : IPasswordResetComposition
{
    public async Task<ApiResponse<Unit>> SendPasswordResetEmailAsync(string email)
    {
        var passwordResetResponse = await passwordResetService.CreatePasswordResetRequestAsync(email);

        if (!passwordResetResponse.IsSuccess)
        {
            return ApiResponse<Unit>.Failure(passwordResetResponse.StatusCode,
                $"Failed to create password reset request: {passwordResetResponse.Message}");
        }

        var emailResponse = await emailService.SendEmailAsync(new EmailSendPasswordResetDto(passwordResetResponse.Value), email);

        if (!emailResponse.IsSuccess)
        {
            await passwordResetService.DeletePasswordResetRequestAsync(passwordResetResponse.Value!.Id);
            
            return ApiResponse<Unit>.Failure(passwordResetResponse.StatusCode,
                $"Failed to send Email: {passwordResetResponse.Message}. Password reset request creation was rolled back.");
        }

        return ApiResponse<Unit>.Success(HttpStatusCode.OK,
            Unit.Value,
            "Email sent successfully");
    }
}