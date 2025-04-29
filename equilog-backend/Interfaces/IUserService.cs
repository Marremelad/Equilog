using equilog_backend.Common;
using equilog_backend.DTOs.UserDTOs;

namespace equilog_backend.Interfaces;

public interface IUserService
{
    Task<ApiResponse<List<UserDto>?>> GetUsersAsync();

    Task<ApiResponse<UserDto?>> GetUserAsync(int userId);
    
    Task<ApiResponse<Unit>> UpdateUserAsync(UserUpdateDto userUpdateDto);

    Task<ApiResponse<Unit>> DeleteUserAsync(int userId);
}