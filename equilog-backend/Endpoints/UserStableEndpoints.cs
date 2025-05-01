using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints
{
    public class UserStableEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/api/userstables/{id:int}", GetUserStable)
                .WithName("GetUserStable");
        }
        private static async Task<IResult> GetUserStable(IUserService userService)
        {
            return Result.Generate(await userService.GetUserStableAsync());
        }
    }
}