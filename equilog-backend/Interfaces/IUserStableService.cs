using equilog_backend.Common;
using equilog_backend.DTOs.UserStableDTOs;
using equilog_backend.Models;

namespace equilog_backend.Interfaces;

public interface IUserStableService
{
    Task<ApiResponse<List<UserStableDto>?>> GetUserStablesAsync(int userId);

    Task<ApiResponse<List<StableUserDto>?>> GetStableUsersAsync(int stableId);

    Task<ApiResponse<Unit>> UpdateStableUserRoleAsync(int userStableId, int userStableRole);
    
    Task<ApiResponse<Unit>> CreateUserStableConnectionAsync(int userId, int stableId);
    
    Task<ApiResponse<Unit>> LeaveStableAsync(int userId, int stableId);
    
    Task<ApiResponse<Unit>> RemoveUserFromStableAsync(int userStableId);

    Task<List<UserStable>> GetConnectionsWithOwnerRole(int userId);

    Task<bool> HasOnlyOneUser(int stableId);

    Task<bool> HasMoreThanOneOwner(UserStable connection);

    Task<UserStable> FindAdminOrUser(int stableId, int excludeUserId);

    Task SetRoleToOwner(UserStable connection);
}