using System.Net;
using equilog_backend.Common;
using equilog_backend.Interfaces;
using equilog_backend.Security;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace equilog_backend.Services;

public class EmailService(SendGridClient client, TwilioSettings twilioSettings) : IEmailService
{
    public async Task<ApiResponse<string?>> SendEmailAsync (IEmail email, string recipient)
    {
        try
        {
            var from = new EmailAddress(email.SenderEmail, email.SenderName);;
            var to = new EmailAddress(recipient);
            var message = MailHelper.CreateSingleEmail(from, to, email.Subject, plainTextContent: email.PlainTextMessage, htmlContent: email.HtmlMessage);
            var response = await client.SendEmailAsync(message);
            
            if (!response.IsSuccessStatusCode) return ApiResponse<string?>.Failure(HttpStatusCode.InternalServerError,
                "Error sending email");
            
            return ApiResponse<string?>.Success(HttpStatusCode.OK,
                email.PlainTextMessage,
                "Email sent successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<string?>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public Task<bool> SendVerificationCodeAsync(string userEmail)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyVerificationCodeAsync(string userEmail, string code)
    {
        throw new NotImplementedException();
    }

    // Request Twilio verification code from Twilio Verify API.
    // public async Task<bool> SendVerificationCode(string userEmail)
    // {
    //     try
    //     {
    //         var verification = await VerificationResource.CreateAsync(
    //             to: userEmail,
    //             channel: "Email",
    //             pathServiceSid: twilioSettings.VerifySid
    //         );
    //         return verification.Status == "pending";
    //     }
    //     catch (ApiException ex)
    //     {
    //         Console.Error.WriteLine($"Twilio API error (Code={ex.Code}): {ex.Message}");
    //         throw new Exception($"Twilio error sending code: {ex.Message}", ex);
    //     }
    // }

    // Verify Twilio verification code.
    // public async Task<bool> VerifyVerificationCode(string userEmail, string code)
    // {
    //     try
    //     {
    //         var verificationCheck = await VerificationCheckResource.CreateAsync(
    //             to: userEmail,
    //             code: code,
    //             pathServiceSid: twilioSettings.VerifySid
    //         );
    //         return verificationCheck.Status == "approved";
    //     }
    //     catch (ApiException)
    //     {
    //         return false;
    //     }
    // }
}