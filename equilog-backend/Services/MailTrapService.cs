using System.Net;
using System.Net.Mail;
using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Services;

public class MailTrapService : IMailTrapService
{
    // Used if the sendgrid api is not available.
    public ApiResponse<Unit> SendEmail(IMailTrap mailTrap, string recipient)
    {
        try
        {
            var client = new SmtpClient("live.smtp.mailtrap.io", 587)
            {
                // Add credentials when this service is used.
                EnableSsl = true
            };
            client.Send("hello@demomailtrap.co", recipient, mailTrap.Subject, mailTrap.Body);

            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "Mail sent successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}