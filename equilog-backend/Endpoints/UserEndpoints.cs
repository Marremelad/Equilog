using System.Net;
using equilog_backend.Common;
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
                .WithName("GetUsers");

            // Get user.
            app.MapGet("/api/user/{id:int}", GetUser)
                .WithName("GetUser");

            // Create user.
            app.MapPost("/api/user/create", CreateUser)
                .WithName("CreateUser");

            // Update user properties.
            app.MapPut("/api/user/update", UpdateUser)
                .WithName("UpdateUser");

            // Delete user.
            app.MapDelete("/api/user/delete/{id:int}", DeleteUser)
                .WithName("DeleteUser");
        }

        private static async Task<IResult> GetUsers(IUserService userService)
        {
            return Response.Generate(await userService.GetUsers());
        }

        private static async Task<IResult> GetUser(IUserService userService, int id)
        {
            return Response.Generate(await userService.GetUser(id));
        }

        private static async Task<IResult> CreateUser(IUserService userService, UserCreateDto newUser)
        {
            return Response.Generate(await userService.CreateUser(newUser));
        }

        private static async Task<IResult> UpdateUser(IUserService userService, UserUpdateDto updatedUser)
        {
            return Response.Generate(await userService.UpdateUser(updatedUser));
        }

        private static async Task<IResult> DeleteUser(IUserService userService, int id)
        {
            return Response.Generate(await userService.DeleteUser(id));
        }
    }
}