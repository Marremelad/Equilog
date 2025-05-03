using equilog_backend.Common;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.DTOs.UserStableDTOs;

namespace equilog_backend.Interfaces
{
    public interface IUserStableService
    {
        Task<ApiResponse<List<UserStableDto>?>> GetUserStablesAsync(int userId);
    }
}
