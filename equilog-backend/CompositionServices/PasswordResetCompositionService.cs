using System.Net;
using equilog_backend.Common;
using equilog_backend.DTOs.EmailDTOs;
using equilog_backend.DTOs.PasswordResetDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Interfaces.CompositionInterfaces;

namespace equilog_backend.CompositionServices;

public class PasswordResetCompositionService(IPasswordResetService passwordResetService, IEmailService emailService) 
    : IPasswordResetCompositionService
{
    public async Task<ApiResponse<PasswordResetDto?>> SendPasswordResetEmailAsync(int id, string recipient)
    {
        var passwordResetRequestResponse = await passwordResetService.CreatePasswordResetRequestAsync(id);

        if (!passwordResetRequestResponse.IsSuccess)
        {
            return ApiResponse<PasswordResetDto>.Failure(passwordResetRequestResponse.StatusCode,
                $"Failed to create password reset request: {passwordResetRequestResponse.Message}");
        }

        var emailResponse = await emailService.SendEmailAsync(new EmailPasswordResetDto(passwordResetRequestResponse.Value), recipient);

        if (!emailResponse.IsSuccess)
        {
            await passwordResetService.DeletePasswordResetRequestAsync(passwordResetRequestResponse.Value!.Id);
            
            return ApiResponse<PasswordResetDto?>.Failure(passwordResetRequestResponse.StatusCode,
                $"Failed to send Email: {passwordResetRequestResponse.Message}. Password reset request creation was rolled back.");
        }

        return ApiResponse<PasswordResetDto?>.Success(HttpStatusCode.OK,
            passwordResetRequestResponse.Value,
            "Email sent successfully");
    }
}