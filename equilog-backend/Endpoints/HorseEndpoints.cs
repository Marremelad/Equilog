using System.Net;
using equilog_backend.Common;
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
        
        // Get Horse.
        app.MapGet("/api/horse/{id:int}", GetHorse)
            .WithName("GetHorse");
        
        // Create horse.
        app.MapPost("/api/horse/create", CreateHorse)
            .WithName("CreateHorse");
        
        // Update horse properties.
        app.MapPut("/api/horse/update", UpdateHorse)
            .WithName("UpdateHorse");

        // Delete horse.
        app.MapDelete("/api/horse/delete/{id:int}", DeleteHorse)
            .WithName("DeleteHorse");
    }

    private static async Task<IResult> GetHorses(IHorseService horseService)
    {
        return Result.Generate(await horseService.GetHorses());
    }

    private static async Task<IResult> GetHorse(IHorseService horseService, int id)
    {
        return Result.Generate(await horseService.GetHorse(id));
    }

    private static async Task<IResult> CreateHorse(IHorseService horseService, HorseCreateDto newHorse)
    {
        return Result.Generate(await horseService.CreateHorse(newHorse));
    }

    private static async Task<IResult> UpdateHorse(IHorseService horseService, HorseUpdateDto updatedHorse)
    {
        return Result.Generate(await horseService.UpdateHorse(updatedHorse));
    }

    private static async Task<IResult> DeleteHorse(IHorseService horseService, int id)
    {
        return Result.Generate(await horseService.DeleteHorse(id));
    }
}
