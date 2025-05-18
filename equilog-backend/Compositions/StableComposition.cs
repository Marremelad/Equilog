using equilog_backend.Common;
using equilog_backend.DTOs.StableCompositionDtos;
using equilog_backend.Interfaces;
using System.Net;

namespace equilog_backend.Compositions;

public class StableComposition(
    IStableService stableService,
    IUserStableService userStableService) : IStableComposition
{
    public async Task<ApiResponse<Unit>> CreateStableCompositionAsync(StableCompositionCreateDto stableCompositionCreateDto)
    {
        var createStable = await stableService.CreateStableAsync(stableCompositionCreateDto.Stable);

        if (!createStable.IsSuccess)
        {
            return ApiResponse<Unit>.Failure(
                createStable.StatusCode,
                $"Failed to create stable: {createStable.Message}");
        }

        var stableId = createStable.Value;
        var userId = stableCompositionCreateDto.UserId;

        var createUserStable = await userStableService.CreateUserStableConnectionAsync(userId, stableId);

        if (!createUserStable.IsSuccess)
        {
            await stableService.DeleteStableAsync(stableId);
            return createUserStable;
        }
        
        return ApiResponse<Unit>.Success(
            HttpStatusCode.Created,
            Unit.Value,
            "Stable created successfully.");
    }
}