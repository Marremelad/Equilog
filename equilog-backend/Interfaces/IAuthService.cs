using equilog_backend.Common;
using equilog_backend.DTOs.AuthDTOs;

namespace equilog_backend.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto?>> RegisterAsync(RegisterDto registerDto);
    
    Task<ApiResponse<AuthResponseDto?>> LoginAsync(LoginDto loginDto);

    Task<ApiResponse<Unit>> ValidatePasswordAsync(ValidatePasswordDto validatePasswordDto);
    
    Task<ApiResponse<AuthResponseDto?>> RefreshTokenAsync(string refreshToken);
    
    Task<ApiResponse<Unit>> RevokeRefreshTokenAsync(string refreshToken);
}