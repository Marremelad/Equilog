using System.Net;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints
{
    public class UserEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            
            // Get all users.
            app.MapGet("/api/user", GetUsers)
                .WithName("Getusers");

            // Get user.
            app.MapGet("/api/user/{id:int}", GetUser)
                .WithName("GetUser");

            // Create user.
            app.MapPost("/api/user", CreateUser)
                .WithName("CreateUser");

            // Update user properties.
            app.MapPut("/api/user", UpdateUser)
                .WithName("UpdateUser");

            // Delete user.
            app.MapDelete("/api/user/{id:int}", DeleteUser)
                .WithName("DeleteUser");
        }

        private static async Task<IResult> GetUsers(IUserService userService)
        {
            var apiResponse = await userService.GetUsers();

            return apiResponse.StatusCode switch
            {
                HttpStatusCode.OK => Results.Ok(apiResponse.Value),
                _ => Results.Problem(apiResponse.Message, statusCode: 500)
            };
        }

        private static async Task<IResult> GetUser(IUserService userService, int id)
        {
            var apiResponse = await userService.GetUser(id);

            return apiResponse.StatusCode switch
            {
                HttpStatusCode.OK => Results.Ok(apiResponse.Value),
                HttpStatusCode.NotFound => Results.NotFound(apiResponse),
                _ => Results.Problem(apiResponse.Message, statusCode: 500)
            };
        }

        private static async Task<IResult> CreateUser(IUserService userService, UserCreateDto newUser)
        {
            var apiResponse = await userService.CreateUser(newUser);

            return apiResponse.StatusCode switch
            {
                HttpStatusCode.Created => Results.Json(apiResponse, statusCode: 201),
                _ => Results.Problem(apiResponse.Message, statusCode: 500)
            };
        }

        private static async Task<IResult> UpdateUser(IUserService userService, UserUpdateDto updatedUser)
        {
            var apiResponse = await userService.UpdateUser(updatedUser);

            return apiResponse.StatusCode switch
            {
                HttpStatusCode.OK => Results.Ok(apiResponse),
                HttpStatusCode.NotFound => Results.NotFound(apiResponse),
                _ => Results.Problem(apiResponse.Message, statusCode: 500)
            };
        }

        private static async Task<IResult> DeleteUser(IUserService userService, int id)
        {
            var apiResponse = await userService.DeleteUser(id);

            return apiResponse.StatusCode switch
            {
                HttpStatusCode.NoContent => Results.Json(apiResponse, statusCode: 204),
                HttpStatusCode.NotFound => Results.NotFound(apiResponse),
                _ => Results.Problem(apiResponse.Message, statusCode: 500)
            };
        }
    }
}
