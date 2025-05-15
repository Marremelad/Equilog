using equilog_backend.Common;
using equilog_backend.DTOs.HorseCompositionDTOs;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class HorseEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get all horses.
        app.MapGet("/api/horse", GetHorses)
            .WithName("GetHorses");
        // .RequireAuthorization();

        // Get Horse.
        app.MapGet("/api/horse/{id:int}", GetHorse)
            .WithName("GetHorse");

        // Get horse profile
        app.MapGet("/api/horse/{horseId:int}/profile", GetHorseProfile)
            .WithName("GetHorseProfile");

        // Create horse.
        app.MapPost("/api/horse/create", CreateHorse)
            .AddEndpointFilter<ValidationFilter<HorseCreateDto>>()
            .WithName("CreateHorse");

        // Update horse properties.
        app.MapPut("/api/horse/update", UpdateHorse)
            .AddEndpointFilter<ValidationFilter<HorseUpdateDto>>()
            .WithName("UpdateHorse");

        // Delete horse.
        app.MapDelete("/api/horse/delete/{id:int}", DeleteHorse)
            .WithName("DeleteHorse");

        // -- Endpoints for compositions --

        // Create a horse with required relations.
        app.MapPost("/api/horse/create/composition", CreateHorseComposition)
            .AddEndpointFilter<ValidationFilter<HorseCompositionCreateDto>>()
            .WithName("CreateHorseComposition");
    }

    private static async Task<IResult> GetHorses(
        IHorseService horseService)
    {
        return Result.Generate(await horseService.GetHorsesAsync());
    }

    private static async Task<IResult> GetHorse(
        IHorseService horseService,
        int id)
    {
        return Result.Generate(await horseService.GetHorseAsync(id));
    }

    private static async Task<IResult> GetHorseProfile(
        IHorseService horseService,
        int horseId)
    {
        return Result.Generate(await horseService.GetHorseProfileAsync(horseId));
    }

    private static async Task<IResult> CreateHorse(
        IHorseService horseService,
        HorseCreateDto newHorse)
    {
        return Result.Generate(await horseService.CreateHorseAsync(newHorse));
    }

    private static async Task<IResult> UpdateHorse(
        IHorseService horseService,
        HorseUpdateDto updatedHorse)
    {
        return Result.Generate(await horseService.UpdateHorseAsync(updatedHorse));
    }

    private static async Task<IResult> DeleteHorse(
        IHorseService horseService,
        int id)
    {
        return Result.Generate(await horseService.DeleteHorseAsync(id));
    }

    private static async Task<IResult> CreateHorseComposition(
        IHorseComposition horseComposition,
        HorseCompositionCreateDto horseCompositionCreateDto)
    {
        return Result.Generate(await horseComposition.CreateHorseCompositionAsync(horseCompositionCreateDto));
    }
}
