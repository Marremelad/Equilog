using System.Net;
using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Compositions;

// Composition service that handles complex user deletion and stable leaving operations.
// Manages ownership transfer and stable cleanup when users are removed from the system.
public class UserStableStableCompositions(
    IUserStableService userStableService,
    IUserService userService,
    IStableService stableService) : IUserStableComposition
{
    // Deletes a user from the system while properly handling stable ownership transfers.
    public async Task<ApiResponse<Unit>> DeleteUserCompositionAsync(int userId)
    {
        try
        {
            // Step 1: Handle ownership transfer for any stables the user owns.
            var transferResponse = await TransferStableOwnership(userId);

            // If the ownership transfer fails, abort the user deletion process.
            if (!transferResponse.IsSuccess)
                return transferResponse;

            // Step 2: Delete the user after ownership has been properly transferred.
            var userResponse = await userService.DeleteUserAsync(userId);

            // If user deletion fails, return the error response.
            if (!userResponse.IsSuccess)
                return userResponse;
            
            // Both operations successful - return success response.
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

    // Removes a user from a specific stable while handling ownership transfer if necessary.
    public async Task<ApiResponse<Unit>> LeaveStableComposition(int userId, int stableId)
    {
        try
        {
            // Step 1: Handle ownership transfer for any stables the user owns.
            var transferResponse = await TransferStableOwnership(userId);

            // If the ownership transfer fails, abort the leave stable process.
            if (!transferResponse.IsSuccess)
                return transferResponse;

            // Step 2: Remove the user from the stable after ownership transfer.
            var userStableResponse = await userStableService.LeaveStableAsync(userId, stableId);

            // If leaving the stable fails, return the error response.
            if (!userStableResponse.IsSuccess)
                return transferResponse;
            
            // Both operations successful - return success response.
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

    // Handles the complex logic of transferring stable ownership when a user is being removed.
    private async Task<ApiResponse<Unit>> TransferStableOwnership(int userId)
    {
        try
        {
            // Get all stable connections where the user has an owner role (role 0).
            var connections = await userStableService.GetConnectionsWithOwnerRole(userId);
        
            // If the user doesn't own any stables, no transfer needed.
            if (connections.Count == 0)
                return ApiResponse<Unit>.Success(
                    HttpStatusCode.OK,
                    Unit.Value,
                    null);

            // Process each stable connection where the user is an owner.
            foreach (var connection in connections)
            {
                // If the user is the only member of the stable, delete the entire stable.
                if (await userStableService.HasOnlyOneUser(connection.StableIdFk))
                {
                    var deleteStable = await stableService.DeleteStableAsync(connection.StableIdFk);

                    // If stable deletion fails, return the error.
                    if (!deleteStable.IsSuccess)
                        return deleteStable;
                    
                    // Continue to the next stable connection.
                    continue;
                }
                
                // If there are multiple owners, no transfer needed for this stable.
                if (await userStableService.HasMoreThanOneOwner(connection))
                    continue;

                // Find an admin or regular user to promote to owner role.
                // This ensures the stable always has at least one owner.
                await userStableService.SetRoleToOwner(await userStableService.FindAdminOrUser(connection.StableIdFk, userId));
            }
            
            // All ownership transfers completed successfully.
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