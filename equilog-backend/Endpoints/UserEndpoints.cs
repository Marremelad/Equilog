using equilog_backend.Common.Enums;
using equilog_backend.Services;

namespace equilog_backend.Endpoints
{
    public class UserEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            app.MapGet("/users", async (UserService userService) =>
            {
                var response = await userService.GetAllUsers();

                return response.OperationResult switch
                {
                    OperationResult.Success => Results.Ok(response.Value),
                    _ => Results.Problem(response.Error, statusCode: 500)
                };
            });
        }
    }
}
