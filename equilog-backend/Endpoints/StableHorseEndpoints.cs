using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class StableHorseEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get connection between stable and horses.
        app.MapGet("/api/stable-horses/{stableId:int}", GetStableHorses) // "/api/stables/{stableId:int}/horses"
            .WithName("GetStableHorses");

        // Get horse and owner information.
        app.MapGet("/api/stable-horses/{stableId:int}/horses/with-owners", GetHorsesWithOwnersByStableId) // "/api/stables/{stableId:int}/horses?include=owners"
            .WithName("GetHorsesWithOwnersByStable");

        // Remove the connection between stable and horse.
        app.MapGet("/api/stable-horse/remove-horse/{stableHorseId:int}", RemoveHorseFromStable) // "/api/stable-horses/{stableHorseId:int}"
            .WithName("RemoveHorseFromStable");
    }

    private static async Task<IResult> GetStableHorses(
        IStableHorseService stableHorseService,
        int stableId)
    {
        return Result.Generate(await stableHorseService.GetStableHorsesAsync(stableId));
    }

    private static async Task<IResult> GetHorsesWithOwnersByStableId(
        IStableHorseService stableHorseService,
        int stableId)
    {
        return Result.Generate(await stableHorseService.GetHorsesWithOwnersByStableIdAsync(stableId));
    }

    private static async Task<IResult> RemoveHorseFromStable(
        IStableHorseService stableHorseService,
        int stableHorseId)
    {
        return Result.Generate(await stableHorseService.RemoveHorseFromStableAsync(stableHorseId));
    }
}