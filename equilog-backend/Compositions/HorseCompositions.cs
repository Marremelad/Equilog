using equilog_backend.Common;
using equilog_backend.DTOs.HorseCompositionDTOs;
using equilog_backend.Interfaces;
using System.Net;

namespace equilog_backend.Compositions;

public class HorseCompositions(
    IHorseService horseService,
    IStableHorseService stableHorseService,
    IUserHorseService userHorseService) : IHorseComposition
{
    public async Task<ApiResponse<Unit>> CreateHorseCompositionAsync(
        HorseCompositionCreateDto horseCompositionCreateDto)
    {
        try
        {
            var horseResponse = await horseService.CreateHorseAsync(horseCompositionCreateDto.Horse);

            if (!horseResponse.IsSuccess)
                return ApiResponse<Unit>.Failure(
                    horseResponse.StatusCode,
                    $"Failed to create horse: {horseResponse.Message}");

            var horseId = horseResponse.Value;
            var stableId = horseCompositionCreateDto.StableId;
            var userId = horseCompositionCreateDto.UserId;

            var stableHorseResponse = await stableHorseService.CreateStableHorseConnectionAsync(stableId, horseId);

            if (!stableHorseResponse.IsSuccess)
            {
                await horseService.DeleteHorseAsync(horseId);
                stableHorseResponse.Message =
                    $"Failed to create connection between stable and horse. Horse creation was rolled back: {stableHorseResponse.Message}";
                return stableHorseResponse;
            }
        
            var userHorseResponse = await userHorseService.CreateUserHorseConnectionAsync(userId, horseId);

            if (!userHorseResponse.IsSuccess)
            {
                await horseService.DeleteHorseAsync(horseId);
                userHorseResponse.Message =
                    $"Failed to create connection between user and horse. Horse creation was rolled back: {userHorseResponse.Message}";
            }

            return ApiResponse<Unit>.Success(
                HttpStatusCode.Created,
                Unit.Value,
                "Horse created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}