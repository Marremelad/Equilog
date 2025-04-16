
using equilog_backend.Common;
using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints
{
    public class WallPostEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/api/wallpost/{stableId:int}", GetWallPost)
                .WithName("GetWallPost");

            app.MapPut("/api/wallpost/replace", ReplaceWallPost)
                .WithName("ReplaceWallPost");

            app.MapPatch("/api/wallpost/edit", EditWallPost)
                .WithName("EditWallPost");

            app.MapPut("/api/wallpost/clear", ClearWallPost)
                .WithName("ClearWallPost");
        }

        private static async Task<IResult> GetWallPost(IWallPostService wallPostService, int stableId)
        {
            return Result.Generate(await wallPostService.GetWallPost(stableId));
        }

        private static async Task<IResult> ReplaceWallPost(IWallPostService wallPostService, WallPostReplaceDto wallPostReplaceDto)
        {
            return Result.Generate(await wallPostService.ReplaceWallPost(wallPostReplaceDto));
        }

        private static async Task<IResult> EditWallPost(IWallPostService wallPostService, WallPostEditDto wallPostEditDto)
        {
            return Result.Generate(await wallPostService.EditWallPost(wallPostEditDto));
        }

        private static async Task<IResult> ClearWallPost(IWallPostService wallPostService, int stableId)
        {
            return Result.Generate(await wallPostService.ClearWallPost(stableId));
        }
    }
}
