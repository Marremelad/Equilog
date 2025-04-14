using System.Net;
using equilog_backend.Common;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class StableEnpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get all stables.
        app.MapGet("/api/stable", GetStables)
            .WithName("GetStables");
        
        // Get stable.
        app.MapGet("/api/stable/{id:int}", GetStable)
            .WithName("GetStable");
        
        // Create stable.
        app.MapPost("/api/stable/create", CreateStable)
            .WithName("CreateStable");
        
        // Update stable.
        app.MapPut("/api/stable/update", UpdateStable)
            .WithName("UpdateStable");
        
        // Delete stable.
        app.MapDelete("/api/stable/delete/{id:int}", DeleteStable)
            .WithName("DeleteStable");
    }

    private static async Task<IResult> GetStables(IStableService stableService)
    {
        return Response.Generate(await stableService.GetStables());
    }

    private static async Task<IResult> GetStable(IStableService stableService, int id)
    {
        return Response.Generate(await stableService.GetStable(id));
    }

    private static async Task<IResult> CreateStable(IStableService stableService, StableCreateDto newStable)
    {
        return Response.Generate(await stableService.CreateStable(newStable));
    }

    private static async Task<IResult> UpdateStable(IStableService stableService, StableUpdateDto updatedStable)
    {
        return Response.Generate(await stableService.UpdateStable(updatedStable));
    }

    private static async Task<IResult> DeleteStable(IStableService stableService, int id)
    {
        return Response.Generate(await stableService.DeleteStable(id));
    }
}