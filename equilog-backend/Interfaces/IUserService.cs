using equilog_backend.Common;
using equilog_backend.DTOs.UserDTOs;

namespace equilog_backend.Interfaces;

public interface IUserService
{
    public Task<ApiResponse<List<UserDto>?>> GetUsers();

    public Task<ApiResponse<UserDto?>> GetUser(int id);

    public Task<ApiResponse<UserDto?>> CreateUser(UserCreateDto newUser);

    public Task<ApiResponse<UserDto?>> UpdateUser(UserUpdateDto updatedUser);

    public Task<ApiResponse<UserDto?>> DeleteUser(int id);
}