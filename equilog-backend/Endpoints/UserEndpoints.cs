using equilog_backend.Common;
using equilog_backend.DTOs.BlobStorageDTOs;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class UserEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    { 
        // Get user.
        app.MapGet("/api/user/{userId:int}", GetUser) // "/api/users/{userId:int}"
            .WithName("GetUser");

        // Get user profile.
        app.MapGet("/api/user/{userId:int}/stable/{stableId:int}", GetUserProfile) // "/api/users/{userId:int}/stables/{stableId:int}/profile"
            .WithName("GetUserProfile");

        // Update user properties.
        app.MapPut("/api/user/update", UpdateUser) // "/api/users/{userId:int}"
            .AddEndpointFilter<ValidationFilter<UserUpdateDto>>()
            .WithName("UpdateUser");

        // Delete user.
        app.MapDelete("/api/user/delete/{userId:int}", DeleteUser) // "/api/users/{userId:int}"
            .WithName("DeleteUser");

        // Set the user's profile picture.
        app.MapPost("/api/user/set-profile-picture", SetProfilePicture) // "/api/users/{userId:int}/profile-picture"
            .WithName("SetProfilePicture");
        
        // Get all users.
        app.MapGet("/api/user", GetUsers) // "/api/users"
            .WithName("GetUsers");
            
        // -- Endpoints for compositions --
        app.MapDelete("/api/user/delete/composition/{userId:int}", DeleteUserComposition) // "/api/users/{userId:int}/compositions"
            .WithName("DeleteUserComposition");
    }
    
    private static async Task<IResult> GetUser(
        IUserService userService,
        int userId)
    {
        return Result.Generate(await userService.GetUserAsync(userId));
    }

    private static async Task<IResult> GetUserProfile(
        IUserService userService,
        int userId, int stableId)
    {
        return Result.Generate(await userService.GetUserProfileAsync(userId, stableId));
    }

    private static async Task<IResult> UpdateUser(
        IUserService userService,
        UserUpdateDto updatedUser)
    {
        return Result.Generate(await userService.UpdateUserAsync(updatedUser));
    }

    private static async Task<IResult> DeleteUser(
        IUserService userService,
        int userId)
    {
        return Result.Generate(await userService.DeleteUserAsync(userId));
    }

    private static async Task<IResult> DeleteUserComposition(
        IUserStableComposition userStableComposition,
        int userId)
    {
        return Result.Generate(await userStableComposition.DeleteUserCompositionAsync(userId));
    }

    private static async Task<IResult> SetProfilePicture(
        IUserService userService,
        BlobStorageDto blobStorageDto)
    {
        return Result.Generate(
            await userService.SetProfilePictureAsync(blobStorageDto.UserId, blobStorageDto.BlobName));
    }
    
    private static async Task<IResult> GetUsers(
        IUserService userService)
    {
        return Result.Generate(await userService.GetUsersAsync());
    }
}