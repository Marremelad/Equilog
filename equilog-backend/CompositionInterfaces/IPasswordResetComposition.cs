using equilog_backend.Common;
using equilog_backend.CompositionDTOs;
using equilog_backend.DTOs.PasswordResetDTOs;

namespace equilog_backend.CompositionInterfaces;

public interface IPasswordResetComposition
{
    public Task<ApiResponse<Unit>> SendPasswordResetEmailAsync(string email);
}