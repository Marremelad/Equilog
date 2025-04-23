using equilog_backend.Common;
using equilog_backend.DTOs.UserDTOs;

namespace equilog_backend.Interfaces;

public interface IUserService
{
    public Task<ApiResponse<List<UserDto>?>> GetUsersAsync();

    public Task<ApiResponse<UserDto?>> GetUserAsync(int id);
    
    public Task<ApiResponse<UserDto?>> UpdateUserAsync(UserUpdateDto userUpdateDto);

    public Task<ApiResponse<UserDto?>> DeleteUserAsync(int id);
}