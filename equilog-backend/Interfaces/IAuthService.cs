using equilog_backend.Common;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.Models;

namespace equilog_backend.Interfaces;

public interface IAuthService
{
    string GenerateJwt(User user);
    Task<ApiResponse<AuthResponseDto?>> RegisterAsync(RegisterDto registerDto);
    Task<ApiResponse<AuthResponseDto?>> LoginAsync(LoginDto loginDto);
}