using equilog_backend.Common;
using equilog_backend.DTOs.PasswordDTOs;
using equilog_backend.DTOs.PasswordResetDTOs;

namespace equilog_backend.Interfaces;

public interface IPasswordService
{
    Task<ApiResponse<PasswordResetRequestDto?>> CreatePasswordResetRequestAsync(string email);
    
    Task<ApiResponse<Unit>> ChangePasswordAsync(PasswordChangeDto passwordChangeDto);

    Task<ApiResponse<Unit>> ResetPasswordAsync(PasswordResetDto passwordResetDto);
    
    Task<ApiResponse<Unit>> DeletePasswordResetRequestAsync(int passwordResetRequestId);
}