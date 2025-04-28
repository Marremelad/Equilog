using equilog_backend.Common;
using equilog_backend.DTOs.PasswordResetDTOs;
using equilog_backend.Models;

namespace equilog_backend.Interfaces;

public interface IPasswordResetService
{
    Task<ApiResponse<PasswordResetRequestDto?>> CreatePasswordResetRequestAsync(string email);
    
    Task<ApiResponse<Unit>> ValidateResetCodeAsync(ValidateResetCodeDto validateResetCodeDto);

    Task<ApiResponse<Unit>> ResetPasswordAsync(PasswordResetDto passwordResetDto);
    
    Task<ApiResponse<Unit>> DeletePasswordResetRequestAsync(int id);
}