using equilog_backend.Common;
using equilog_backend.DTOs.EmailDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class EmailEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Send Twilio email.
        app.MapPost("/api/twilio-mail/send/", SendEmail)
            .AddEndpointFilter<ValidationFilter<EmailDto>>()
            .WithName("SendTwilioMail");
    }

    private static async Task<IResult> SendEmail(IEmailService emailService, EmailDto emailDto)
    {
        return Result.Generate(await emailService.SendEmailAsync(new EmailWelcomeDto(), emailDto.Email));
    }
}