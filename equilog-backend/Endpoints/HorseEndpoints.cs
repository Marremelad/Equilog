using equilog_backend.Common;
using equilog_backend.DTOs.HorseCompositionDTOs;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class HorseEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get horse profile
        app.MapGet("/api/horse/{horseId:int}/profile", GetHorseProfile) // "/api/horses/{horseId:int}/profile"
            .RequireAuthorization()
            .WithName("GetHorseProfile");

        // Create horse.
        app.MapPost("/api/horse/create", CreateHorse) // "/api/horses"
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<HorseCreateDto>>()
            .WithName("CreateHorse");

        // Update horse properties.
        app.MapPut("/api/horse/update", UpdateHorse) // "/api/horses/{horseId:int}"
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<HorseUpdateDto>>()
            .WithName("UpdateHorse");

        // Delete horse.
        app.MapDelete("/api/horse/delete/{horseId:int}", DeleteHorse) // "/api/horses/{horseId:int}"
            .RequireAuthorization()
            .WithName("DeleteHorse");
        
        // -- Endpoints for compositions --

        // Create a horse with required relations.
        app.MapPost("/api/horse/create/composition", CreateHorseComposition) // "/api/horses/compositions"
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<HorseCompositionCreateDto>>()
            .WithName("CreateHorseComposition");
        
        // -- Endpoints for testing --
        
        // Get all horses.
        app.MapGet("/api/horse", GetHorses) // "/api/horses"
            .RequireAuthorization()
            .WithName("GetHorses");
        
        // Get Horse.
        app.MapGet("/api/horse/{horseId:int}", GetHorse) // "/api/horses/{horseId:int}"
            .RequireAuthorization()
            .WithName("GetHorse");
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
        int horseId)
    {
        return Result.Generate(await horseService.DeleteHorseAsync(horseId));
    }

    private static async Task<IResult> CreateHorseComposition(
        IHorseComposition horseComposition,
        HorseCompositionCreateDto horseCompositionCreateDto)
    {
        return Result.Generate(await horseComposition.CreateHorseCompositionAsync(horseCompositionCreateDto));
    }
    
    // Used for testing.
    private static async Task<IResult> GetHorses(
        IHorseService horseService)
    {
        return Result.Generate(await horseService.GetHorsesAsync());
    }
    
    private static async Task<IResult> GetHorse(
        IHorseService horseService,
        int horseId)
    {
        return Result.Generate(await horseService.GetHorseAsync(horseId));
    }
}
