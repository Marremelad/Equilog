using System.Net;
using equilog_backend.Common;
using equilog_backend.CompositionInterfaces;
using equilog_backend.DTOs.EmailDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.CompositionServices;

public class PasswordResetComposition(IPasswordResetService passwordResetService, IEmailService emailService) 
    : IPasswordResetComposition
{
    public async Task<ApiResponse<Unit>> SendPasswordResetEmailAsync(string email)
    {
        var passwordResetRequestResponse = await passwordResetService.CreatePasswordResetRequestAsync(email);

        if (!passwordResetRequestResponse.IsSuccess)
        {
            return ApiResponse<Unit>.Failure(passwordResetRequestResponse.StatusCode,
                $"Failed to create password reset request: {passwordResetRequestResponse.Message}");
        }

        var emailResponse = await emailService.SendEmailAsync(new EmailPasswordResetDto(passwordResetRequestResponse.Value), email);

        if (!emailResponse.IsSuccess)
        {
            await passwordResetService.DeletePasswordResetRequestAsync(passwordResetRequestResponse.Value!.Id);
            
            return ApiResponse<Unit>.Failure(passwordResetRequestResponse.StatusCode,
                $"Failed to send Email: {passwordResetRequestResponse.Message}. Password reset request creation was rolled back.");
        }

        return ApiResponse<Unit>.Success(HttpStatusCode.OK,
            Unit.Value,
            "Email sent successfully");
    }
}