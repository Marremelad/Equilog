using equilog_backend.Common;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Compositions;

public class UserComposition : IUserComposition
{
    public async Task<ApiResponse<Unit>> DeleteUserCompositionAsync(UserDeleteDto userDeleteDto)
    {
        throw new NotImplementedException();
    }
}