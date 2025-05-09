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

            app.MapGet("/api/userstables/stable/{stableId:int}", GetStableUsers)
                .WithName("GetStableUsers");

            app.MapPut("/api/userstables/stable-user/{stableUserId:int}", UpdateStableUserRole)
                .WithName("UpdateStableUserRole");
        }
        private static async Task<IResult> GetUserStables(IUserStableService userStableService, int userId)
        {
            return Result.Generate(await userStableService.GetUserStablesAsync(userId));
        }

        private static async Task<IResult> GetStableUsers(IUserStableService userStableService, int stableId)
        {
            return Result.Generate(await userStableService.GetStableUsersAsync(stableId));
        }

        private static async Task<IResult> UpdateStableUserRole(IUserStableService userStableService, int userStableId, int userStableRole)
        {
            return Result.Generate(await userStableService.UpdateStableUserRoleAsync(userStableId, userStableRole));
        }
    }
}