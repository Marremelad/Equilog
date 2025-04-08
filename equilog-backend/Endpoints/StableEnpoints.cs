using System.Net;
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

    private static async Task<IResult> CreateStable(IStableService stableService, StableCreateDto newStable)
    {
        var apiResponse = await stableService.CreateStable(newStable);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.Created => Results.Json(apiResponse, statusCode: 201),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> UpdateStable(IStableService stableService, StableUpdateDto updatedStable)
    {
        var apiResponse = await stableService.UpdateStable(updatedStable);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> DeleteStable(IStableService stableService, int id)
    {
        var apiResponse = await stableService.DeleteStable(id);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.NoContent => Results.Json(apiResponse, statusCode: 204),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }
}