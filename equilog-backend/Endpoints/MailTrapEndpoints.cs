using equilog_backend.Common;
using equilog_backend.DTOs.MailTrapDTOs;
using equilog_backend.Services;

namespace equilog_backend.Endpoints;

// Used if the regular email endpoints stop working. 
public class MailTrapEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Send MailTrap email.
        app.MapPost("/api/mail-trap-send/welcome", SendEmail) // "/api/mail-trap-emails/welcome"
            .RequireAuthorization()
            .WithName("SendMailTrapEmail");
    }

    private static IResult SendEmail(
        MailTrapService mailTrapService,
        string recipient)
    {
        return Result.Generate(mailTrapService.SendEmail(new MailTrapWelcomeDto(), recipient));
    }
}