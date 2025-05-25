using equilog_backend.Common;
using equilog_backend.DTOs.EmailDTOs;
using equilog_backend.DTOs.EmailSendDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class EmailEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Send Email.
        app.MapPost("/api/email-send/welcome/", SendEmail) // "/api/emails/welcome"
            .AddEndpointFilter<ValidationFilter<EmailDto>>()
            .WithName("SendTwilioMail");
        
        // -- Endpoints for compositions --
        
        // Send password reset email.
        app.MapPost("/api/password-reset-email/send", SendPasswordResetEmail) // "/api/password-resets"
            .AddEndpointFilter<ValidationFilter<EmailDto>>()
            .WithName("SendPasswordResetEmail");
    }

    private static async Task<IResult> SendEmail(
        IEmailService emailService,
        EmailDto emailDto)
    {
        return Result.Generate(await emailService.SendEmailAsync(new EmailSendWelcomeDto(), emailDto.Email));
    }
    
    // -- Result generators for compositions --
    private static async Task<IResult> SendPasswordResetEmail(
        IPasswordResetComposition passwordResetComposition,
        EmailDto emailDto)
    {
        return Result.Generate(await passwordResetComposition.SendPasswordResetEmailAsync(emailDto.Email));
    }
}