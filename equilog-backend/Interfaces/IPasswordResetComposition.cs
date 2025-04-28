using equilog_backend.Common;

namespace equilog_backend.Interfaces;

public interface IPasswordResetComposition
{
    public Task<ApiResponse<Unit>> SendPasswordResetEmailAsync(string email);
}