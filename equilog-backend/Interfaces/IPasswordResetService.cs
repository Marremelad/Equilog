using equilog_backend.Common;
using equilog_backend.DTOs.PasswordResetDTOs;
using equilog_backend.Models;

namespace equilog_backend.Interfaces;

public interface IPasswordResetService
{
    public Task<ApiResponse<PasswordResetRequestDto?>> CreatePasswordResetRequest(int id);
}