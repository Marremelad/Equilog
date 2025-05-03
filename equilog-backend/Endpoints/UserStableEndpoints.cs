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
        }
        private static async Task<IResult> GetUserStables(IUserStableService userStableService, int userId)
        {
            return Result.Generate(await userStableService.GetUserStablesAsync(userId));
        }
    }
}