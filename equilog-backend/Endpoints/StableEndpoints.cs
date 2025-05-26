using equilog_backend.Common;
using equilog_backend.DTOs.StableCompositionDtos;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class StableEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get stable by stableId.
        app.MapGet("/api/stable/{stableId:int}", GetStableByStableId) // "/api/stables/{stableId:int}"
            .RequireAuthorization()
            .WithName("GetStableByStableId");

        // Get stables by name.
        app.MapGet("/api/stable/search", SearchStables) // "/api/stables"
            .RequireAuthorization()
            .WithName("SearchStables");

        // Update stable.
        app.MapPut("/api/stable/update", UpdateStable) // "/api/stables/{stableId:int}"
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<StableUpdateDto>>()
            .WithName("UpdateStable");

        // Delete stable.
        app.MapDelete("/api/stable/delete/{stableId:int}", DeleteStable) // "/api/stables/{stableId:int}"
            .RequireAuthorization()
            .WithName("DeleteStable");

        // -- Endpoints for compositions --

        // Create stable with required components and relations.
        app.MapPost("/api/stable/create", CreateStableComposition) // "/api/stables"
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<StableCompositionCreateDto>>()
            .WithName("CreateStable");
    }

    private static async Task<IResult> GetStableByStableId(
        IStableService stableService,
        int stableId)
    {
        return Result.Generate(await stableService.GetStableByStableIdAsync(stableId));
    }

    private static async Task<IResult> SearchStables(
        IStableService stableService,
        [AsParameters] StableSearchParametersDto stableSearchParametersDto)
    {
        return Result.Generate(await stableService.SearchStablesAsync(stableSearchParametersDto));
    }

    private static async Task<IResult> UpdateStable(
        IStableService stableService,
        StableUpdateDto updatedStable)
    {
        return Result.Generate(await stableService.UpdateStableAsync(updatedStable));
    }

    private static async Task<IResult> DeleteStable(
        IStableService stableService,
        int stableId)
    {
        return Result.Generate(await stableService.DeleteStableAsync(stableId));
    }

    // -- Result generators for compositions --
    private static async Task<IResult> CreateStableComposition(
        IStableComposition stableComposition,
        StableCompositionCreateDto stableCompositionCreateDto)
    {
        var result = await stableComposition.CreateStableCompositionAsync(stableCompositionCreateDto);
        return Result.Generate(result);
    }
}