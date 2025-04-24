using equilog_backend.Common;
using equilog_backend.DTOs.EmailDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class EmailEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/api/twilio-mail/send", SendEmail)
            .WithName("SendTwilioMail");
    }

    private static async Task<IResult> SendEmail(IEmailService emailService, string recipient)
    {
        return Result.Generate(await emailService.SendEmailAsync(new EmailWelcomeDto(), recipient));
    }
}