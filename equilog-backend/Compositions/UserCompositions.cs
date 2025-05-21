using System.Net;
using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Compositions;

public class UserCompositions(
    IUserStableService userStableService,
    IUserService userService,
    IStableService stableService) : IUserComposition
{
    public async Task<ApiResponse<Unit>> DeleteUserCompositionAsync(int userId)
    {
        try
        {
            var transferStableOwnership = await TransferStableOwnership(userId);

            if (!transferStableOwnership.IsSuccess)
                return transferStableOwnership;

            var deleteUser = await userService.DeleteUserAsync(userId);

            if (!deleteUser.IsSuccess)
                return deleteUser;
            
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                "User deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    private async Task<ApiResponse<Unit>> TransferStableOwnership(int userId)
    {
        try
        {
            var connections = await userStableService.GetConnectionsWithOwnerRole(userId);
        
            if (connections.Count == 0)
                return ApiResponse<Unit>.Success(HttpStatusCode.OK,
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
            
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}