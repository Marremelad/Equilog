using equilog_backend.Common;
using equilog_backend.DTOs.UserDTOs;

namespace equilog_backend.Interfaces;

public interface IUserStableComposition
{
    Task<ApiResponse<Unit>> DeleteUserCompositionAsync(int userId);

    Task<ApiResponse<Unit>> LeaveStableComposition(int userId, int stableId);
}