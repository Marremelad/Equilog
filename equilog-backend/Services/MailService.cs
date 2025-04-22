using System.Net;
using System.Net.Mail;
using equilog_backend.Common;
using equilog_backend.DTOs.MailDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Services;

public class MailService
{
    public ApiResponse<string?> SendMail(IMail mail, string recipient)
    {
        try
        {
            var client = new SmtpClient("live.smtp.mailtrap.io", 587)
            {
                Credentials = new NetworkCredential("api", "f22eb97375cb24dce3f488034c22baab"),
                EnableSsl = true
            };
            client.Send("hello@demomailtrap.co", recipient, mail.Subject, mail.Body);

            return ApiResponse<string>.Success(HttpStatusCode.OK,
                mail.Body,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<string>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}