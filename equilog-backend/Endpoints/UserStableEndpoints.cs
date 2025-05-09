using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints
{
    public class UserStableEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/api/userstables/user/{userId:int}", GetUserStables)
                .WithName("GetUserStables");

            app.MapGet("/api/userstables/stableId/{stableId:int}", GetStableUsers)
                .WithName("GetStableUsers");
        }
        private static async Task<IResult> GetUserStables(IUserStableService userStableService, int userId)
        {
            return Result.Generate(await userStableService.GetUserStablesAsync(userId));
        }

        private static async Task<IResult> GetStableUsers(IUserStableService userStableService, int stableId)
        {
            return Result.Generate(await userStableService.GetStableUsersAsync(stableId));
        }
    }
}