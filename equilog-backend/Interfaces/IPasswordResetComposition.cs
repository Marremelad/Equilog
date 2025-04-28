using equilog_backend.Common;

namespace equilog_backend.Interfaces;

public interface IPasswordResetComposition
{
    Task<ApiResponse<Unit>> SendPasswordResetEmailAsync(string email);
}