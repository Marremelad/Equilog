using equilog_backend.Common;

namespace equilog_backend.Interfaces;

public interface IEmailService
{
    Task<ApiResponse<Unit>> SendEmailAsync (IEmail email, string recipient);
}