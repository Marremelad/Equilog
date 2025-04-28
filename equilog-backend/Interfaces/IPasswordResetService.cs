using equilog_backend.Common;
using equilog_backend.DTOs.PasswordResetDTOs;
using equilog_backend.Models;

namespace equilog_backend.Interfaces;

public interface IPasswordResetService
{
    public Task<ApiResponse<PasswordResetRequestDto?>> CreatePasswordResetRequestAsync(string email);
    
    public Task<ApiResponse<Unit>> ValidateResetCodeAsync(ValidateResetCodeDto validateResetCodeDto);

    public Task<ApiResponse<Unit>> ResetPasswordAsync(PasswordResetDto passwordResetDto);
    
    public Task<ApiResponse<Unit>> DeletePasswordResetRequestAsync(int id);
}