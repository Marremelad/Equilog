using System.Net;
using equilog_backend.Common;
using equilog_backend.CompositionInterfaces;
using equilog_backend.DTOs.EmailDTOs;
using equilog_backend.DTOs.PasswordResetDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.CompositionServices;

public class PasswordResetCompositionService(IPasswordResetService passwordResetService, IEmailService emailService) 
    : IPasswordResetCompositionService
{
    public async Task<ApiResponse<PasswordResetRequestDto?>> SendPasswordResetEmailAsync(string email)
    {
        var passwordResetRequestResponse = await passwordResetService.CreatePasswordResetRequestAsync(email);

        if (!passwordResetRequestResponse.IsSuccess)
        {
            return ApiResponse<PasswordResetRequestDto>.Failure(passwordResetRequestResponse.StatusCode,
                $"Failed to create password reset request: {passwordResetRequestResponse.Message}");
        }

        var emailResponse = await emailService.SendEmailAsync(new EmailPasswordResetDto(passwordResetRequestResponse.Value), email);

        if (!emailResponse.IsSuccess)
        {
            await passwordResetService.DeletePasswordResetRequestAsync(passwordResetRequestResponse.Value!.Id);
            
            return ApiResponse<PasswordResetRequestDto?>.Failure(passwordResetRequestResponse.StatusCode,
                $"Failed to send Email: {passwordResetRequestResponse.Message}. Password reset request creation was rolled back.");
        }

        return ApiResponse<PasswordResetRequestDto?>.Success(HttpStatusCode.OK,
            passwordResetRequestResponse.Value,
            "Email sent successfully");
    }
}