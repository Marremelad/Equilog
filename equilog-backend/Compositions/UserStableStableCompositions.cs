using System.Net;
using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Compositions;

public class UserStableStableCompositions(
    IUserStableService userStableService,
    IUserService userService,
    IStableService stableService) : IUserStableComposition
{
    public async Task<ApiResponse<Unit>> DeleteUserCompositionAsync(int userId)
    {
        try
        {
            var transferResponse = await TransferStableOwnership(userId);

            if (!transferResponse.IsSuccess)
                return transferResponse;

            var userResponse = await userService.DeleteUserAsync(userId);

            if (!userResponse.IsSuccess)
                return userResponse;
            
            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "User deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> LeaveStableComposition(int userId, int stableId)
    {
        try
        {
            var transferResponse = await TransferStableOwnership(userId);

            if (!transferResponse.IsSuccess)
                return transferResponse;

            var userStableResponse = await userStableService.LeaveStableAsync(userId, stableId);

            if (!userStableResponse.IsSuccess)
                return transferResponse;
            
            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "User left stable successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    private async Task<ApiResponse<Unit>> TransferStableOwnership(int userId)
    {
        try
        {
            var connections = await userStableService.GetConnectionsWithOwnerRole(userId);
        
            if (connections.Count == 0)
                return ApiResponse<Unit>.Success(
                    HttpStatusCode.OK,
                    Unit.Value,
                    null);

            foreach (var connection in connections)
            {
                // Skip if there's only one user in the stable (the owner themselves),
                if (await userStableService.HasOnlyOneUser(connection.StableIdFk))
                {
                    var deleteStable = await stableService.DeleteStableAsync(connection.StableIdFk);

                    if (!deleteStable.IsSuccess)
                        return deleteStable;
                    
                    continue;
                }
                
                // Skip if there's already more than one owner.
                if (await userStableService.HasMoreThanOneOwner(connection))
                    continue;

                // We know there are multiple users and only one owner, so this will find someone.
                await userStableService.SetRoleToOwner(await userStableService.FindAdminOrUser(connection.StableIdFk, userId));
            }
            
            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}