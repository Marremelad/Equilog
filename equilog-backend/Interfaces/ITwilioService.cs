using equilog_backend.Common;

namespace equilog_backend.Interfaces
{
    public interface ITwilioService
    {
        Task<ApiResponse<string?>> SendEmailAsync (string recipientEmail, IEmail email);
        Task<bool> SendVerificationCodeAsync(string userEmail);
        Task<bool> VerifyVerificationCodeAsync(string userEmail, string code);
    }
}
