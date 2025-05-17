using equilog_backend.Common;
using equilog_backend.DTOs.UserDTOs;

namespace equilog_backend.Interfaces;

public interface IUserComposition
{
    Task<ApiResponse<Unit>> DeleteUserCompositionAsync(UserDeleteDto userDeleteDto);
    
}