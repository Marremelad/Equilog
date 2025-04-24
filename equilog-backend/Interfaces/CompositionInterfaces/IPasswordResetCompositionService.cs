using equilog_backend.Common;
using equilog_backend.DTOs.PasswordResetDTOs;

namespace equilog_backend.Interfaces.CompositionInterfaces;

public interface IPasswordResetCompositionService
{
    public Task<ApiResponse<PasswordResetDto?>> SendPasswordResetEmailAsync(int id, string recipient);
}