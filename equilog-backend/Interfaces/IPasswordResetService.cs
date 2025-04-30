using equilog_backend.Common;
using equilog_backend.DTOs.PasswordResetDTOs;
using equilog_backend.Models;

namespace equilog_backend.Interfaces;

public interface IPasswordResetService
{
    Task<ApiResponse<PasswordResetRequestDto?>> CreatePasswordResetRequestAsync(string email);
    
    Task<ApiResponse<Unit>> ValidateResetTokenAsync(ValidateResetTokenDto validateResetTokenDto);

    Task<ApiResponse<Unit>> ResetPasswordAsync(PasswordResetDto passwordResetDto);

    Task<ApiResponse<Unit>> ResetPasswordWithTokenAsync(PasswordResetWithTokenDto passwordResetWithTokenDto);
    
    Task<ApiResponse<Unit>> DeletePasswordResetRequestAsync(int passwordResetRequestId);
}