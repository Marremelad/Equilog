using equilog_backend.Common;

namespace equilog_backend.Interfaces
{
    public interface ITwilioService
    {
        Task<ApiResponse<string?>> SendEmail (string toEmail, string subject, string plainTextMessage, string htmlMessage);
        Task<bool> SendWelcomeEmail(string toEmail);
        Task<bool> SendVerificationCode(string userEmail);
        Task<bool> VerifyVerificationCode(string userEmail, string code);
    }
}
