using equilog_backend.Common;
using equilog_backend.DTOs.UserStableDTOs;

namespace equilog_backend.Interfaces
{
    public interface IUserStableService
    {
        Task<ApiResponse<List<UserStableDto>?>> GetUserStablesAsync(int userId);

        Task<ApiResponse<List<StableUserDto>?>> GetStableUsersAsync(int stableId);

        Task<ApiResponse<Unit>> UpdateStableUserRoleAsync(int userStableId, int userStableRole);

        Task<ApiResponse<Unit>> RemoveUserFromStableAsync(int userStableId);

        Task<ApiResponse<Unit>> CreateUserStableConnectionOnStableCreation(int userId, int stableId);
    }
}
