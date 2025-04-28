using System.Net;
using System.Net.Mail;
using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Services;

public class MailTrapService : IMailTrapService
{
    public ApiResponse<Unit> SendEmail(IMailTrap mailTrap, string recipient)
    {
        try
        {
            var client = new SmtpClient("live.smtp.mailtrap.io", 587)
            {
                Credentials = new NetworkCredential("api", "f22eb97375cb24dce3f488034c22baab"),
                EnableSsl = true
            };
            client.Send("hello@demomailtrap.co", recipient, mailTrap.Subject, mailTrap.Body);

            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}