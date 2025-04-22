using equilog_backend.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using Twilio.Exceptions;
using Twilio.Rest.Verify.V2.Service;

namespace equilog_backend.Services
{
    public class TwilioService(string twilioVerifySid, string apiKey) : ITwilioService
    {
        private readonly string fromName = "Equilog";
        private readonly string fromEmail = "pontus.dorsay@gmail.com";
        private readonly SendGridClient client = new(apiKey);

        // Generall adjustable email.
        public async Task<bool> SendCustomizableEmailAsync(string toEmail, string subject, string plainTextMessage, string htmlMessage)
        {

            var from = new EmailAddress(fromEmail, fromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: plainTextMessage, htmlContent: htmlMessage);
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }

        // Welcome mail for new users.
        public async Task<bool> SendWelcomeMailAsync(string toEmail)
        {
            var from = new EmailAddress(fromEmail, fromName);
            var to = new EmailAddress(toEmail);
            var subject = "Welcome to Equilog";
            var plainTextContent = "Hi there,\r\n\r\nWelcome to Equilog! We're excited to have you on board.\r\n\r\nEquilog is designed to make horse management easier, smarter, and more connected. You're now part of a community that cares deeply about horses and their well-being.\r\n\r\nIf you have any questions or feedback, just reply to this email. We're here to help.\r\n\r\nHappy riding,  \r\nThe Equilog Team\r\n";
            var htmlContent = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <title>Welcome to Equilog</title>\r\n</head>\r\n<body style=\"font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 30px;\">\r\n  <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px; background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 10px rgba(0,0,0,0.1); overflow: hidden;\">\r\n    <tr>\r\n      <td style=\"background-color: #4CAF50; padding: 20px; text-align: center;\">\r\n        <h1 style=\"color: #ffffff; margin: 0;\">Welcome to Equilog!</h1>\r\n      </td>\r\n    </tr>\r\n    <tr>\r\n      <td style=\"padding: 30px;\">\r\n        <p style=\"font-size: 18px; color: #333;\">Hi there 👋</p>\r\n        <p style=\"font-size: 16px; color: #555;\">\r\n          We're thrilled to have you join the Equilog community — a space built for horse lovers, riders, and caretakers like you.\r\n        </p>\r\n        <p style=\"font-size: 16px; color: #555;\">\r\n          With Equilog, managing your horses has never been easier. From logging activities to keeping health records, we’ve got your back.\r\n        </p>\r\n        <p style=\"font-size: 16px; color: #555;\">\r\n          If you ever need help or just want to say hi, feel free to reply to this email. We'd love to hear from you.\r\n        </p>\r\n        <p style=\"font-size: 16px; color: #4CAF50; font-weight: bold;\">\r\n          Happy riding,<br>The Equilog Team 🐎\r\n        </p>\r\n      </td>\r\n    </tr>\r\n    <tr>\r\n      <td style=\"padding: 20px; text-align: center; background-color: #f0f0f0; font-size: 12px; color: #999;\">\r\n        &copy; 2025 Equilog. All rights reserved.<br>\r\n        You received this email because you signed up for Equilog.\r\n      </td>\r\n    </tr>\r\n  </table>\r\n</body>\r\n</html>\r\n";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }

        // Request Twilio verification code from Twilio Verify API.
        public async Task<bool> SendVerificationCodeAsync(string userEmail)
        {
            try
            {
                var verification = await VerificationResource.CreateAsync(
                    to: userEmail,
                    channel: "Email",
                    pathServiceSid: twilioVerifySid
                );
                return verification.Status == "pending";
            }
            catch (ApiException ex)
            {
                Console.Error.WriteLine($"Twilio API error (Code={ex.Code}): {ex.Message}");
                throw new Exception($"Twilio error sending code: {ex.Message}", ex);
            }
        }

        // Verify Twilio verification code.
        public async Task<bool> VerifyVerificationCodeAsync(string userEmail, string code)
        {
            try
            {
                var verificationCheck = await VerificationCheckResource.CreateAsync(
                    to: userEmail,
                    code: code,
                    pathServiceSid: twilioVerifySid
                );
                return verificationCheck.Status == "approved";
            }
            catch (ApiException)
            {
                return false;
            }
        }
    }
}
