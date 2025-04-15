
namespace equilog_backend.Endpoints
{
    public class WallPostEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/api/wallpost", GetWallPost)
                .WithName("GetWallPost");

            app.MapPut("/api/wallpost/replace", ReplaceWallPost)
                .WithName("ReplaceWallPost");

            app.MapPatch("/api/wallpost/edit", EditWallPost)
                .WithName("EditWallPost");
        }

        private static async Task EditWallPost(HttpContext context)
        {
            throw new NotImplementedException();
        }

        private static async Task ReplaceWallPost(HttpContext context)
        {
            throw new NotImplementedException();
        }

        private static async Task GetWallPost(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
