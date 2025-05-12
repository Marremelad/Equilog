using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class StableHorseEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/api/stable-horses/{id:int}", GetStableHorses)
            .WithName("GetStableHorses");

        app.MapGet("/api/stable-horse/remove-horse/{id:int}", RemoveHorseFromStable)
            .WithName("RemoveHorseFromStable");
    }

    private static async Task<IResult> GetStableHorses(
        IStableHorseService stableHorseService,
        int id)
    {
        return Result.Generate(await stableHorseService.GetStableHorsesAsync(id));
    }

    private static async Task<IResult> RemoveHorseFromStable(
        IStableHorseService stableHorseService,
        int id)
    {
        return Result.Generate(await stableHorseService.RemoveHorseFromStableAsync(id));
    }
}