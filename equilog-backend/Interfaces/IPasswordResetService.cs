using equilog_backend.Common;
using equilog_backend.DTOs.PasswordResetDTOs;
using equilog_backend.Models;

namespace equilog_backend.Interfaces;

public interface IPasswordResetService
{
    public Task<ApiResponse<PasswordResetRequestDto?>> CreatePasswordResetRequestAsync(string email);

    public Task<ApiResponse<PasswordResetRequestDto?>> DeletePasswordResetRequestAsync(int id);

    public Task<ApiResponse<ValidateResetCodeDto?>> ValidateResetCodeAsync(ValidateResetCodeDto validateResetCodeDto);

    public Task<ApiResponse<PasswordResetDto?>> ResetPasswordAsync(PasswordResetDto passwordResetDto);
}