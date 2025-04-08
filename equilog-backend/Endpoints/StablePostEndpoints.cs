using System.Net;
using equilog_backend.DTOs.StablePostDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class StablePostEndpoints
{
     public static void RegisterEndpoints(WebApplication app)
    {
        // Get all stable posts.
        app.MapGet("/api/stable-post", GetStablePosts)
            .WithName("GetStablePosts");
        
        // Get stable post.
        app.MapGet("/api/stable-post/{id:int}", GetStablePost)
            .WithName("GetStablePost");
        
        // Create stable post.
        app.MapPost("/api/stable-post/create", CreateStablePost)
            .WithName("CreateStablePost");
        
        // Update stable post properties.
        app.MapPut("/api/stable-post/update", UpdateStablePost)
            .WithName("UpdateStablePost");

        // Delete stable post.
        app.MapDelete("/api/stable-post/delete/{id:int}", DeleteStablePost)
            .WithName("DeleteStablePost");
    }

    private static async Task<IResult> GetStablePosts(IStablePostService stablePostService)
    {
        var apiResponse = await stablePostService.GetStablePosts();
            
        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> GetStablePost(IStablePostService stablePostService, int id)
    {
        var apiResponse = await stablePostService.GetStablePost(id);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> CreateStablePost(IStablePostService stablePostService, StablePostCreateDto newStablePost)
    {
        var apiResponse = await stablePostService.CreateStablePost(newStablePost);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.Created => Results.Json(apiResponse, statusCode: 201),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> UpdateStablePost(IStablePostService stablePostService, StablePostUpdateDto updatedStablePost)
    {
        var apiResponse = await stablePostService.UpdateStablePost(updatedStablePost);
            
        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> DeleteStablePost(IStablePostService stablePostService, int id)
    {
        var apiResponse = await stablePostService.DeleteStablePost(id);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.NoContent => Results.Json(apiResponse, statusCode: 204),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }
}