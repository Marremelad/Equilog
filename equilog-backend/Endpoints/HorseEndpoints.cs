using System.Net;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.Services;

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
        app.MapPost("/api/horse", CreateHorse)
            .WithName("CreateHorse");
        
        // Update horse properties.
        app.MapPut("/api/horse", UpdateHorse)
            .WithName("UpdateHorse");

        // Delete horse.
        app.MapDelete("/api/horse/{id:int}", DeleteHorse)
            .WithName("DeleteHorse");
    }

    private static async Task<IResult> GetHorses(HorseService horseService)
    {
        var apiResponse = await horseService.GetHorses();
            
        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> GetHorse(HorseService horseService, int id)
    {
        var apiResponse = await horseService.GetHorse(id);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> CreateHorse(HorseService horseService, HorseCreateDto newHorse)
    {
        var apiResponse = await horseService.CreateHorse(newHorse);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.Created => Results.Json(apiResponse, statusCode: 201),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> UpdateHorse(HorseService horseService, HorseUpdateDto updatedHorse)
    {
        var apiResponse = await horseService.UpdateHorse(updatedHorse);
            
        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> DeleteHorse(HorseService horseService, int id)
    {
        var apiResponse = await horseService.DeleteHorse(id);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.NoContent => Results.Json(apiResponse, statusCode: 204),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }
}