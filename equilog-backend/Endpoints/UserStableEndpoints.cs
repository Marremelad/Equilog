using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints
{
    public class UserStableEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/api/user-stables/user/{id:int}", GetUserStables)
                .WithName("GetUserStables");

            app.MapGet("/api/user-stables/stable/{id:int}", GetStableUsers)
                .WithName("GetStableUsers");

            app.MapPut("/api/user-stables/stable-user/{id:int}", UpdateStableUserRole)
                .WithName("UpdateStableUserRole");
        }
        private static async Task<IResult> GetUserStables(IUserStableService userStableService, int id)
        {
            return Result.Generate(await userStableService.GetUserStablesAsync(id));
        }

        private static async Task<IResult> GetStableUsers(IUserStableService userStableService, int id)
        {
            return Result.Generate(await userStableService.GetStableUsersAsync(id));
        }

        private static async Task<IResult> UpdateStableUserRole(IUserStableService userStableService, int id, int userStableRole)
        {
            return Result.Generate(await userStableService.UpdateStableUserRoleAsync(id, userStableRole));
        }
    }
}