using equilog_backend.Common;
using equilog_backend.DTOs.StablePostDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class StablePostEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get all stable posts.
        app.MapGet("/api/stable-post-by-stable-id/{id:int}", GetStablePosts)
            .WithName("GetStablePosts");

        // Get stable post.
        app.MapGet("/api/stable-post/{id:int}", GetStablePost)
            .WithName("GetStablePost");

        // Create stable post.
        app.MapPost("/api/stable-post/create", CreateStablePost)
            .AddEndpointFilter<ValidationFilter<StablePostCreateDto>>()
            .WithName("CreateStablePost");

        // Update stable post properties.
        app.MapPut("/api/stable-post/update", UpdateStablePost)
            .AddEndpointFilter<ValidationFilter<StablePostUpdateDto>>()
            .WithName("UpdateStablePost");

        // Delete stable post.
        app.MapDelete("/api/stable-post/delete/{id:int}", DeleteStablePost)
            .WithName("DeleteStablePost");
    }

    private static async Task<IResult> GetStablePosts(IStablePostService stablePostService, int id)
    {
        return Result.Generate(await stablePostService.GetStablePostsAsync(id));
    }

    private static async Task<IResult> GetStablePost(IStablePostService stablePostService, int id)
    {
        return Result.Generate(await stablePostService.GetStablePostAsync(id));
    }

    private static async Task<IResult> CreateStablePost(IStablePostService stablePostService, StablePostCreateDto newStablePost)
    {
        return Result.Generate(await stablePostService.CreateStablePostAsync(newStablePost));
    }

    private static async Task<IResult> UpdateStablePost(IStablePostService stablePostService, StablePostUpdateDto updatedStablePost)
    {
        return Result.Generate(await stablePostService.UpdateStablePostAsync(updatedStablePost));
    }

    private static async Task<IResult> DeleteStablePost(IStablePostService stablePostService, int id)
    {
        return Result.Generate(await stablePostService.DeleteStablePostAsync(id));
    }
}