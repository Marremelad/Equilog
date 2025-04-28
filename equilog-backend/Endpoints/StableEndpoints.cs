using equilog_backend.Common;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class StableEndpoints
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
    
    // -- Result generator for compositions --
    private static async Task<IResult> CreateStableWithWallPost(
        IStableComposition stableComposition, 
        StableCreateDto stableCreateDto)
    {
        var result = await stableComposition.CreateStableWithWallPostAsync(stableCreateDto);
        return Result.Generate(result);
    }
}