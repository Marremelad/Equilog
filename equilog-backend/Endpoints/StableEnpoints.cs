using System.Net;
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
    }

    private static async Task<IResult> GetStables(IStableService stableService)
    {
        var apiResponse = await stableService.GetStables();

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> GetStable(IStableService stableService, int id)
    {
        var apiResponse = await stableService.GetStable(id);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }
}