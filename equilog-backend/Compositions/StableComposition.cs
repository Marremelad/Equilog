using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.DTOs.StableCompositionDtos;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Compositions;

public class StableComposition(
    IStableService stableService,
    IWallPostService wallPostService,
    IUserStableService userStableService,
    IMapper mapper) : IStableComposition
{
    public async Task<ApiResponse<Unit>> CreateStableComposition(StableCompositionCreateDto stableCompositionCreateDto)
    {
        var stableCreateDto = mapper.Map<StableCreateDto>(stableCompositionCreateDto);
        
        var stableResponse = await stableService.CreateStableAsync(stableCreateDto);
        
        if (!stableResponse.IsSuccess)
        {
            return ApiResponse<Unit>.Failure(
                stableResponse.StatusCode,
                $"Failed to create stable: {stableResponse.Message}");
        }

        var stableId = stableResponse.Value!.Id;
        var userId = stableCompositionCreateDto.UserId;
        
        var userStableResponse = await userStableService.CreateUserStableConnectionOnStableCreation(userId, stableId);

        if (!userStableResponse.IsSuccess)
        {
            return await RollbackStableCreation(
                stableId, 
                userStableResponse.StatusCode,
                $"Failed to establish connection between user and stable: {userStableResponse.Message}");
        }

        var wallPostResponse = await wallPostService.CreateWallPostAsync(stableId);
        
        if (!wallPostResponse.IsSuccess)
        {
            return await RollbackStableCreation(
                stableId, 
                wallPostResponse.StatusCode,
                $"Failed to create wall post: {wallPostResponse.Message}");
        }
        
        return ApiResponse<Unit>.Success(
            HttpStatusCode.Created,
            Unit.Value,
            "Stable created successfully.");
    }
    
    private async Task<ApiResponse<Unit>> RollbackStableCreation(int stableId, HttpStatusCode statusCode, string errorMessage)
    {
        await stableService.DeleteStableAsync(stableId);
        return ApiResponse<Unit>.Failure(statusCode, $"{errorMessage}. Stable creation was rolled back.");
    }
}