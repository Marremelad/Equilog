using equilog_backend.Common;
using equilog_backend.DTOs.StableCompositionDtos;
using equilog_backend.Interfaces;
using System.Net;

namespace equilog_backend.Compositions;

// Composition service that orchestrates stable creation with user ownership assignment.
// Handles the complex operation of creating a stable and linking it to the creating user as an owner.
public class StableCompositions(
    IStableService stableService,
    IUserStableService userStableService) : IStableComposition
{
    // Creates a complete stable composition including the stable entity and user-stable relationship.
    public async Task<ApiResponse<Unit>> CreateStableCompositionAsync(StableCompositionCreateDto stableCompositionCreateDto)
    {
        try
        {
            // Step 1: Create the core stable entity.
            var stableResponse = await stableService.CreateStableAsync(stableCompositionCreateDto.Stable);

            // If stable creation fails, return early without creating relationships.
            if (!stableResponse.IsSuccess)
            {
                return ApiResponse<Unit>.Failure(
                    stableResponse.StatusCode,
                    $"Failed to create stable: {stableResponse.Message}");
            }

            // Extract IDs for creating the user-stable relationship.
            var stableId = stableResponse.Value;
            var userId = stableCompositionCreateDto.UserId;

            // Step 2: Create the user-stable relationship (assign ownership to the user).
            var userStableResponse = await userStableService.CreateUserStableConnectionAsync(userId, stableId);

            // If the user-stable relationship fails, rollback by deleting the stable.
            if (!userStableResponse.IsSuccess)
            {
                // Clean up the created stable since the ownership assignment failed.
                await stableService.DeleteStableAsync(stableId);
                userStableResponse.Message =
                    $"Failed to create connection between user and stable: {userStableResponse.Message}. Stable creation was rolled back.";
                return userStableResponse;
            }
        
            // Both operations successful - return success response.
            return ApiResponse<Unit>.Success(
                HttpStatusCode.Created,
                Unit.Value,
                "Stable created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}
