using System.Net;
using equilog_backend.Common;
using equilog_backend.CompositionDTOs;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Interfaces.CompositionInterfaces;

namespace equilog_backend.CompositionServices;

public class StableCompositionService(IStableService stableService, IWallPostService wallPostService) : IStableCompositionService
{
    public async Task<ApiResponse<StableWithWallPostDto?>> CreateStableWithWallPostAsync(StableCreateDto stableCreateDto)
    {
        var stableResponse = await stableService.CreateStableAsync(stableCreateDto);
        
        if (!stableResponse.IsSuccess)
        {
            return ApiResponse<StableWithWallPostDto>.Failure(
                stableResponse.StatusCode,
                $"Failed to create stable: {stableResponse.Message}");
        }

        var wallPostResponse = await wallPostService.CreateWallPostAsync(stableResponse.Value!.Id);
        
        if (!wallPostResponse.IsSuccess)
        {
            await stableService.DeleteStableAsync(stableResponse.Value.Id);
            
            return ApiResponse<StableWithWallPostDto>.Failure(
                wallPostResponse.StatusCode,
                $"Failed to create wall post: {wallPostResponse.Message}. Stable creation was rolled back.");
        }
        
        var result = new StableWithWallPostDto
        {
            StableDto = stableResponse.Value,
            WallPostDto = wallPostResponse.Value
        };

        return ApiResponse<StableWithWallPostDto>.Success(
            HttpStatusCode.Created,
            result,
            "Stable and wall post created successfully.");
    }
}