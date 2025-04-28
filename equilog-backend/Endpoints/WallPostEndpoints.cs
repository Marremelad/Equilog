using equilog_backend.Common;
using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints
{
    public class WallPostEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            // Get wall post.
            app.MapGet("/api/wallpost/{stableId:int}", GetWallPost)
                .WithName("GetWallPost");

            // Replace wall post.
            app.MapPut("/api/wallpost/replace", ReplaceWallPost)
                .WithName("ReplaceWallPost");

            // Edit wall post.
            app.MapPatch("/api/wallpost/edit", EditWallPost)
                .WithName("EditWallPost");

            // Clear wall post.
            app.MapPut("/api/wallpost/clear", ClearWallPost)
                .WithName("ClearWallPost");

            // Create wall post.
            app.MapPost("/api/wallpost/create", CreateWallPost)
                .WithName("CreateWallPost");
        }

        private static async Task<IResult> GetWallPost(IWallPostService wallPostService, int stableId)
        {
            return Result.Generate(await wallPostService.GetWallPostAsync(stableId));
        }

        private static async Task<IResult> ReplaceWallPost(IWallPostService wallPostService, WallPostReplaceDto wallPostReplaceDto)
        {
            return Result.Generate(await wallPostService.ReplaceWallPostAsync(wallPostReplaceDto));
        }

        private static async Task<IResult> EditWallPost(IWallPostService wallPostService, WallPostEditDto wallPostEditDto)
        {
            return Result.Generate(await wallPostService.EditWallPostAsync(wallPostEditDto));
        }

        private static async Task<IResult> ClearWallPost(IWallPostService wallPostService, int stableId)
        {
            return Result.Generate(await wallPostService.ClearWallPostAsync(stableId));
        }

        private static async Task<IResult> CreateWallPost(IWallPostService wallPostService, int stableId)
        {
            return Result.Generate(await wallPostService.CreateWallPostAsync(stableId));
        }
    }
}
