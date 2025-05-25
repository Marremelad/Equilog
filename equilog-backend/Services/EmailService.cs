using System.Net;
using equilog_backend.Common;
using equilog_backend.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace equilog_backend.Services;

// Service that handles email sending functionality using SendGrid email service.
// Provides reliable email delivery for notifications, password resets, and other communications.
public class EmailService(SendGridClient client) : IEmailService
{
    // Sends an email using SendGrid with both plain text and HTML content.
    public async Task<ApiResponse<Unit>> SendEmailAsync (IEmail email, string recipient)
    {
        try
        {
            // Creates a sender email address object from the email DTO.
            var from = new EmailAddress(email.SenderEmail, email.SenderName);
            
            // Create recipient email address object.
            var to = new EmailAddress(recipient);
            
            // Build the email message with subject, plain text, and HTML content.
            var message = MailHelper.CreateSingleEmail(
                from,
                to,
                email.Subject,
                plainTextContent: email.PlainTextMessage,
                htmlContent: email.HtmlMessage);
            
            // Send the email through SendGrid API.
            var response = await client.SendEmailAsync(message);
            
            // Check if SendGrid returned a successful status code.
            if (!response.IsSuccessStatusCode) return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                "Error: Could not send email.");
            
            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "Email sent successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}