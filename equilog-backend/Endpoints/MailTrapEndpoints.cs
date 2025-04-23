using equilog_backend.Common;
using equilog_backend.DTOs.MailTrapDTOs;
using equilog_backend.Services;

namespace equilog_backend.Endpoints;

public class MailTrapEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/api/mail/send", SendMail)
            .WithName("SendEmail");
    }

    private static IResult SendMail(MailTrapService mailTrapService, string recipient)
    {
        return Result.Generate(mailTrapService.SendEmail(new MailTrapWelcomeDto(), recipient));
    }
}