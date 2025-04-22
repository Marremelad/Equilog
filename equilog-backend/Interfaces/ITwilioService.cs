namespace equilog_backend.Interfaces
{
    public interface ITwilioService
    {
        Task<bool> SendCustomizableEmailAsync(string toEmail, string subject, string plainTextMessage, string htmlMessage);
        Task<bool> SendWelcomeMailAsync(string toEmail);
        Task<bool> SendVerificationCodeAsync(string userEmail);
        Task<bool> VerifyVerificationCodeAsync(string userEmail, string code);
    }
}
