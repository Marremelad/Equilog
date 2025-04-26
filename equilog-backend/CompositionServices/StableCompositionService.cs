using System.Net;
using equilog_backend.Common;
using equilog_backend.CompositionInterfaces;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.CompositionServices;

public class StableCompositionService(IStableService stableService, IWallPostService wallPostService) : IStableCompositionService
{
    public async Task<ApiResponse<Unit>> CreateStableWithWallPostAsync(StableCreateDto stableCreateDto)
    {
        var stableResponse = await stableService.CreateStableAsync(stableCreateDto);
        
        if (!stableResponse.IsSuccess)
        {
            return ApiResponse<Unit>.Failure(
                stableResponse.StatusCode,
                $"Failed to create stable: {stableResponse.Message}");
        }

        var wallPostResponse = await wallPostService.CreateWallPostAsync(stableResponse.Value!.Id);
        
        if (!wallPostResponse.IsSuccess)
        {
            await stableService.DeleteStableAsync(stableResponse.Value.Id);
            
            return ApiResponse<Unit>.Failure(
                wallPostResponse.StatusCode,
                $"Failed to create wall post: {wallPostResponse.Message}. Stable creation was rolled back.");
        }
        
        return ApiResponse<Unit>.Success(
            HttpStatusCode.Created,
            Unit.Value,
            "Stable and wall post created successfully.");
    }
}