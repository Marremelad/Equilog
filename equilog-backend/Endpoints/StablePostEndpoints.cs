using equilog_backend.Common;
using equilog_backend.DTOs.StablePostDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class StablePostEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get all stable posts.
        app.MapGet("/api/stable-post-by-stable-id/{stableId:int}", GetStablePostsByStableId) // "/api/stables/{stableId:int}/posts"
            .WithName("GetStablePosts");

        // Get stable post.
        app.MapGet("/api/stable-post/{stablePostId:int}", GetStablePostByStablePostId) // "/api/stable-posts/{stablePostId:int}"
            .WithName("GetStablePost");

        // Create stable post.
        app.MapPost("/api/stable-post/create", CreateStablePost) // "/api/stable-posts"
            .AddEndpointFilter<ValidationFilter<StablePostCreateDto>>()
            .WithName("CreateStablePost");

        // Update stable post properties.
        app.MapPut("/api/stable-post/update", UpdateStablePost) // "/api/stable-posts/{stablePostId:int}"
            .AddEndpointFilter<ValidationFilter<StablePostUpdateDto>>()
            .WithName("UpdateStablePost");
        
        // Change IsPinned flag.
        app.MapPatch("/api/stable-post/is-pinned/change/{stablePostId:int}", ChangeStablePostIsPinnedFlag) // "/api/stable-posts/{stablePostId:int}/pinned"
            .WithName("ChangeStablePostIsPinnedFlag");

        // Delete stable post.
        app.MapDelete("/api/stable-post/delete/{stablePostId:int}", DeleteStablePost) // "/api/stable-posts/{stablePostId:int}"
            .WithName("DeleteStablePost");
    }

    private static async Task<IResult> GetStablePostsByStableId(
        IStablePostService stablePostService,
        int stableId)
    {
        return Result.Generate(await stablePostService.GetStablePostsByStableIdAsync(stableId));
    }

    private static async Task<IResult> GetStablePostByStablePostId(
        IStablePostService stablePostService,
        int stablePostId)
    {
        return Result.Generate(await stablePostService.GetStablePostByStablePostIdAsync(stablePostId));
    }

    private static async Task<IResult> CreateStablePost(
        IStablePostService stablePostService,
        StablePostCreateDto newStablePost)
    {
        return Result.Generate(await stablePostService.CreateStablePostAsync(newStablePost));
    }

    private static async Task<IResult> UpdateStablePost(
        IStablePostService stablePostService,
        StablePostUpdateDto updatedStablePost)
    {
        return Result.Generate(await stablePostService.UpdateStablePostAsync(updatedStablePost));
    }

    private static async Task<IResult> ChangeStablePostIsPinnedFlag(
        IStablePostService stablePostService,
        int stablePostId)
    {
        return Result.Generate(await stablePostService.ChangeStablePostIsPinnedFlagAsync(stablePostId));
    }

    private static async Task<IResult> DeleteStablePost(
        IStablePostService stablePostService,
        int stablePostId)
    {
        return Result.Generate(await stablePostService.DeleteStablePostAsync(stablePostId));
    }
}