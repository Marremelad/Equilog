using equilog_backend.Common;
using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.Models;

namespace equilog_backend.Interfaces;

public interface IAuthService
{
    string GenerateToke(User user);
    Task<ApiResponse<AuthResponseDto?>> Register(RegisterDto registerDto);
    Task<ApiResponse<AuthResponseDto?>> Login(LoginDto loginDto);
}