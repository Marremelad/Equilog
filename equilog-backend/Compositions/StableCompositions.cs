using equilog_backend.Common;
using equilog_backend.DTOs.StableCompositionDtos;
using equilog_backend.Interfaces;
using System.Net;

namespace equilog_backend.Compositions;

public class StableCompositions(
    IStableService stableService,
    IUserStableService userStableService) : IStableComposition
{
    public async Task<ApiResponse<Unit>> CreateStableCompositionAsync(StableCompositionCreateDto stableCompositionCreateDto)
    {
        try
        {
            var stableResponse = await stableService.CreateStableAsync(stableCompositionCreateDto.Stable);

            if (!stableResponse.IsSuccess)
            {
                return ApiResponse<Unit>.Failure(
                    stableResponse.StatusCode,
                    $"Failed to create stable: {stableResponse.Message}");
            }

            var stableId = stableResponse.Value;
            var userId = stableCompositionCreateDto.UserId;

            var userStableResponse = await userStableService.CreateUserStableConnectionAsync(userId, stableId);

            if (!userStableResponse.IsSuccess)
            {
                await stableService.DeleteStableAsync(stableId);
                userStableResponse.Message =
                    $"Failed to create connection between user and stable: {userStableResponse.Message}. Stable creation was rolled back.";
                return userStableResponse;
            }
        
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
