using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class StableHorseEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/api/stable-horses", GetStableHorses)
            .WithName("GetStableHorses");
    }

    private static async Task<IResult> GetStableHorses(
        IStableHorseService stableHorseService,
        int id)
    {
        return Result.Generate(await stableHorseService.GetStableHorsesAsync(id));
    }
}