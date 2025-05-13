using AutoMapper;
using equilog_backend.Common;
using equilog_backend.DTOs.HorseCompositionDTOs;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.Interfaces;
using System.Net;

namespace equilog_backend.Compositions;

public class HorseComposition(
    IHorseService horseService,
    IStableHorseService stableHorseService,
    IUserHorseService userHorseService,
    IMapper mapper) : IHorseComposition
{
    public async Task<ApiResponse<Unit>> CreateHorseCompositionAsync(HorseCompositionCreateDto horseCompositionCreateDto)
    {
        var horseCreateDto = mapper.Map<HorseCreateDto>(horseCompositionCreateDto.Horse);

        var horseResponse = await horseService.CreateHorseAsync(horseCreateDto);

        if (!horseResponse.IsSuccess)
            return ApiResponse<Unit>.Failure(horseResponse.StatusCode,
                $"Failed to create horse: {horseResponse.Message}");

        var horseId = horseResponse.Value!.Id;
        var stableId = horseCompositionCreateDto.StableId;
        var userId = horseCompositionCreateDto.UserId;

        var stableHorseResponse = await stableHorseService.CreateStableHorseConnectionAsync(stableId, horseId);

        if (!stableHorseResponse.IsSuccess)
        {
            await horseService.DeleteHorseAsync(horseId);
            return ApiResponse<Unit>.Failure(stableHorseResponse.StatusCode,
                $"{stableHorseResponse.Message}: Horse creation was rolled back");
        }

        var stableHorseId = stableHorseResponse.Value;

        var userHorseResponse = await userHorseService.CreateUserHorseConnectionAsync(userId, horseId);

        if (!userHorseResponse.IsSuccess)
        {
            await horseService.DeleteHorseAsync(horseId);
            await stableHorseService.RemoveHorseFromStableAsync(stableHorseId);
            return ApiResponse<Unit>.Failure(userHorseResponse.StatusCode,
                $"{userHorseResponse.Message}: Horse creation and connection between stable and horse was rolled back.");
        }

        return ApiResponse<Unit>.Success(HttpStatusCode.Created,
            Unit.Value,
            "Horse created successfully.");
    }
}