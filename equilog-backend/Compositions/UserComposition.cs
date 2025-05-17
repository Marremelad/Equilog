using System.Net;
using equilog_backend.Common;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Compositions;

public class UserComposition(
    IUserStableService userStableService,
    IUserService userService) : IUserComposition
{
    public async Task<ApiResponse<Unit>> DeleteUserCompositionAsync(UserDeleteDto userDeleteDto)
    {
        var userId = userDeleteDto.UserId;
        var stableId = userDeleteDto.StableId;

        var checkOwners = await userStableService.CheckNumberOfStableOwners(stableId);

        if (checkOwners is { StatusCode: HttpStatusCode.InternalServerError })
            return ApiResponse<Unit>.Failure(checkOwners.StatusCode,
                checkOwners.Message);

        if (checkOwners.IsSuccess)
        {
            await userService.DeleteUserAsync(userId);
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                null);            
        }

        var setRole = await userStableService.SetRoleToOwner(stableId);
        
        if (setRole is { StatusCode: HttpStatusCode.InternalServerError })
            return  ApiResponse<Unit>.Failure(setRole.StatusCode,
                setRole.Message);

        await userService.DeleteUserAsync(userId);
        
        return ApiResponse<Unit>.Success(HttpStatusCode.OK,
            Unit.Value,
            null);
    }
}