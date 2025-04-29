using equilog_backend.Common;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.Models;

namespace equilog_backend.Interfaces;

public interface IAuthService
{
    string CreateJwt(User user);
    Task<ApiResponse<AuthResponseDto?>> RegisterAsync(RegisterDto registerDto);
    Task<ApiResponse<AuthResponseDto?>> LoginAsync(LoginDto loginDto);
    
    Task<RefreshToken> CreateRefreshTokenAsync(int userId);
    bool ValidateRefreshToken(RefreshToken? refreshToken); 
    Task<ApiResponse<AuthResponseDto?>> RefreshTokenAsync(string refreshToken);
    Task<ApiResponse<Unit>> RevokeRefreshTokenAsync(string refreshToken);
}