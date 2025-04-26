using equilog_backend.Common;

namespace equilog_backend.Interfaces
{
    public interface IEmailService
    {
        Task<ApiResponse<Unit>> SendEmailAsync (IEmail email, string recipient);
        Task<bool> SendVerificationCodeAsync(string userEmail);
        Task<bool> VerifyVerificationCodeAsync(string userEmail, string code);
    }
}
