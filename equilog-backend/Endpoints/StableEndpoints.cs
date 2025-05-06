using equilog_backend.Common;
using equilog_backend.DTOs.StableCompositionDtos;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace equilog_backend.Endpoints;

public class StableEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get all stables.
        app.MapGet("/api/stable", GetStables)
            .WithName("GetStables");

        // Get stable by id.
        app.MapGet("/api/stable/{id:int}", GetStable)
            .WithName("GetStable");

        // Get stable(s) by name.
        app.MapGet("/api/stable/search", SearchStables)
            .WithName("SearchStables");

        // Create stable.
        app.MapPost("/api/stable/create", CreateStable)
            .AddEndpointFilter<ValidationFilter<StableCreateDto>>()
            .WithName("CreateStable");

        // Update stable.
        app.MapPut("/api/stable/update", UpdateStable)
            .AddEndpointFilter<ValidationFilter<StableUpdateDto>>()
            .WithName("UpdateStable");

        // Delete stable.
        app.MapDelete("/api/stable/delete/{id:int}", DeleteStable)
            .WithName("DeleteStable");
        
        // -- Endpoints for compositions --
        
        // Create stable with wall post.
        app.MapPost("/api/stable/create-with-wall-post", CreateStableWithWallPost)
            .AddEndpointFilter<ValidationFilter<StableCreateDto>>()
            .WithName("CreateStableWithWallPost");
    }

    private static async Task<IResult> GetStables(IStableService stableService)
    {
        return Result.Generate(await stableService.GetStablesAsync());
    }

    private static async Task<IResult> GetStable(IStableService stableService, int id)
    {
        return Result.Generate(await stableService.GetStableAsync(id));
    }

    public static async Task<IResult> SearchStables(IStableService stableService, [FromQuery] string searchTerm = "", [FromQuery] int page = 0, [FromQuery] int pageSize = 10)
    {
        return Result.Generate(await stableService.SearchStablesAsync(searchTerm, page, pageSize));
    }

    private static async Task<IResult> CreateStable(IStableService stableService, StableCreateDto stableCreateDto)
    {
        return Result.Generate(await stableService.CreateStableAsync(stableCreateDto));
    }

    private static async Task<IResult> UpdateStable(IStableService stableService, StableUpdateDto updatedStable)
    {
        return Result.Generate(await stableService.UpdateStableAsync(updatedStable));
    }

    private static async Task<IResult> DeleteStable(IStableService stableService, int id)
    {
        return Result.Generate(await stableService.DeleteStableAsync(id));
    }
    
    // -- Result generators for compositions --
    private static async Task<IResult> CreateStableWithWallPost(
        IStableComposition stableComposition, 
        StableCompositionCreateDto stableCompositionCreateDto)
    {
        var result = await stableComposition.CreateStableComposition(stableCompositionCreateDto);
        return Result.Generate(result);
    }
}