using System.Net;
using equilog_backend.Common;
using equilog_backend.DTOs.EmailSendDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Security;

namespace equilog_backend.Compositions;

// Composition service that orchestrates password reset functionality by coordinating
// password reset request creation and email notification sending.
public class PasswordResetCompositions(
    IPasswordService passwordService,
    IEmailService emailService,
    PasswordResetSettings passwordResetSettings) 
    : IPasswordResetComposition
{
    
    // Handles the complete password reset flow: creates reset request and sends notification email.
    public async Task<ApiResponse<Unit>> SendPasswordResetEmailAsync(string email)
    {
        // Step 1: Create a password reset request with token and expiration.
        var passwordResetResponse = await passwordService.CreatePasswordResetRequestAsync(email);

        // If password reset request creation fails, return early without sending email.
        if (!passwordResetResponse.IsSuccess)
        {
            return ApiResponse<Unit>.Failure(passwordResetResponse.StatusCode,
                $"Failed to create password reset request: {passwordResetResponse.Message}");
        }

        // Step 2: Send a password-reset email with the generated token and reset URL.
        var emailResponse = await emailService.SendEmailAsync(
            new EmailSendPasswordResetDto(passwordResetResponse.Value, passwordResetSettings.BaseUrl),
            email);

        // If email sending fails, clean up by deleting the password reset request.
        if (!emailResponse.IsSuccess)
        {
            await passwordService.DeletePasswordResetRequestAsync(passwordResetResponse.Value!.Id);
            emailResponse.Message =
                $"Failed to send Email: {emailResponse.Message}. Password reset request creation was rolled back.";
            return emailResponse;
        }

        // Both operations successful - return success response.
        return ApiResponse<Unit>.Success(HttpStatusCode.OK,
            Unit.Value,
            "Email sent successfully");
    }
}