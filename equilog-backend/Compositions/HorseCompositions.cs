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
    public async Task<ApiResponse<Unit>> CreateHorseCompositionAsync(HorseCompositionCreateDto horseCompositionCreateDto)
    {
        try
        {
            var createHorse = await horseService.CreateHorseAsync(horseCompositionCreateDto.Horse);

            if (!createHorse.IsSuccess)
                return ApiResponse<Unit>.Failure(createHorse.StatusCode,
                    $"Failed to create horse: {createHorse.Message}");

            var horseId = createHorse.Value;
            var stableId = horseCompositionCreateDto.StableId;
            var userId = horseCompositionCreateDto.UserId;

            var createStableHorse = await stableHorseService.CreateStableHorseConnectionAsync(stableId, horseId);

            if (!createStableHorse.IsSuccess)
            {
                await horseService.DeleteHorseAsync(horseId);
                return ApiResponse<Unit>.Failure(createStableHorse.StatusCode,
                    $"{createStableHorse.Message}: Horse creation was rolled back");
            }
        
            var createUserHorse = await userHorseService.CreateUserHorseConnectionAsync(userId, horseId);

            if (!createUserHorse.IsSuccess)
            {
                await horseService.DeleteHorseAsync(horseId);
                return ApiResponse<Unit>.Failure(createUserHorse.StatusCode,
                    $"{createUserHorse.Message}: Horse creation and connection between stable and horse was rolled back.");
            }

            return ApiResponse<Unit>.Success(HttpStatusCode.Created,
                Unit.Value,
                "Horse created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}