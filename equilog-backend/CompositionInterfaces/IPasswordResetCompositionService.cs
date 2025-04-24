using equilog_backend.Common;
using equilog_backend.CompositionDTOs;
using equilog_backend.DTOs.PasswordResetDTOs;

namespace equilog_backend.CompositionInterfaces;

public interface IPasswordResetCompositionService
{
    public Task<ApiResponse<PasswordResetRequestDto?>> SendPasswordResetEmailAsync(string email);
}