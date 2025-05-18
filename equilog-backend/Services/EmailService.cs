using System.Net;
using equilog_backend.Common;
using equilog_backend.Interfaces;
using equilog_backend.Security;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace equilog_backend.Services;

public class EmailService(SendGridClient client, TwilioSettings twilioSettings) : IEmailService
{
    public async Task<ApiResponse<Unit>> SendEmailAsync (IEmail email, string recipient)
    {
        try
        {
            var from = new EmailAddress(email.SenderEmail, email.SenderName);;
            var to = new EmailAddress(recipient);
            var message = MailHelper.CreateSingleEmail(from, to, email.Subject, plainTextContent: email.PlainTextMessage, htmlContent: email.HtmlMessage);
            var response = await client.SendEmailAsync(message);
            
            if (!response.IsSuccessStatusCode) return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                "Error sending email");
            
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                "Email sent successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}