using System.Net;
using equilog_backend.Common;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.Services;

namespace equilog_backend.Endpoints;

public class HorseEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get all horses.
        app.MapGet("/api/horse", GetHorses)
            .WithName("GetHorses")
            .Produces<ApiResponse<List<HorseDto>>>()
            .Produces<ApiResponse<List<HorseDto>>>(500); // Look up if this is best practice.
        
        // Get Horse.
        app.MapGet("api/horse/{id:int}", GetHorse)
            .WithName("GetHorse")
            .Produces<ApiResponse<HorseDto>>()
            .Produces(404)
            .Produces(500);
        
        // Create horse.
        app.MapPost("/api/horse", CreateHorse)
            .WithName("CreateHorse")
            .Produces<ApiResponse<HorseDto>>(201)
            .Produces(500);
        
        // Update horse properties.
        app.MapPut("/api/horse", UpdateHorse)
            .WithName("UpdateHorse")
            .Accepts<HorseDto>("application/json")
            .Produces<ApiResponse<HorseDto>>()
            .Produces(404)
            .Produces(500);

        // Delete horse.
        app.MapDelete("/api/horse/{id:int}", DeleteHorse)
            .WithName("DeleteHorse")
            .Produces<ApiResponse<HorseDto>>(204)
            .Produces(404)
            .Produces(500);
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