using System.Net;
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

                return response.StatusCode switch
                {
                    HttpStatusCode.OK => Results.Ok(response.Value),
                    _ => Results.Problem(response.Message, statusCode: 500)
                };
            });
        }
    }
}
