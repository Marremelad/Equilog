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
        app.MapPost("/api/email-send/welcome/", SendEmail)
            .AddEndpointFilter<ValidationFilter<EmailDto>>()
            .WithName("SendTwilioMail");
    }

    private static async Task<IResult> SendEmail(IEmailService emailService, EmailDto emailDto)
    {
        return Result.Generate(await emailService.SendEmailAsync(new EmailSendWelcomeDto(), emailDto.Email));
    }
}