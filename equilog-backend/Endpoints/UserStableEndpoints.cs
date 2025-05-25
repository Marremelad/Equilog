using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class UserStableEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get connections between users and stable by user ID.
        app.MapGet("/api/user-stables/user/{userId:int}", GetUserStablesByUserId) // "/api/users/{userId:int}/stables"
            .WithName("GetUserStablesByUserId");

        // Get connection between user and stable by stable ID.
        app.MapGet("/api/user-stables/stable/{stableId:int}", GetUserStablesByStableId) // "/api/stables/{stableId:int}/users"
            .WithName("GetUserStablesByStableId");

        // Update a user's role in a stable.
        app.MapPut("/api/user-stables/stable-user/{userStableId:int}", UpdateStableUserRole) // "/api/user-stables/{userStableId:int}/role"
            .WithName("UpdateStableUserRole");
            
        // Remove a user from a stable.
        app.MapDelete("/api/user-stables/{userStableId:int}", RemoveUserFromStable) // "/api/user-stables/{userStableId:int}"
            .WithName("RemoveUserFromStable");
            
        // -- Endpoints for compositions --

        // Composition for removing a user from a stable.
        app.MapDelete("/api/user-stables/leave/{userId:int}/{stableId:int}", LeaveStableComposition) // "/api/users/{userId:int}/stables/{stableId:int}"
            .WithName("LeaveStableComposition");
    }
        
    private static async Task<IResult> GetUserStablesByUserId(
        IUserStableService userStableService,
        int userId)
    {
        return Result.Generate(await userStableService.GetUserStablesByUserIdAsync(userId));
    }

    private static async Task<IResult> GetUserStablesByStableId(
        IUserStableService userStableService,
        int stableId)
    {
        return Result.Generate(await userStableService.GetUserStablesByStableIdAsync(stableId));
    }

    private static async Task<IResult> UpdateStableUserRole(
        IUserStableService userStableService,
        int userStableId,
        int userStableRole)
    {
        return Result.Generate(await userStableService.UpdateStableUserRoleAsync(userStableId, userStableRole));
    }

    private static async Task<IResult> LeaveStableComposition(
        IUserStableComposition userStableComposition, 
        int userId,
        int stableId)
    {
        return Result.Generate(await userStableComposition.LeaveStableComposition(userId, stableId));
    }

    private static async Task<IResult> RemoveUserFromStable(
        IUserStableService userStableService,
        int userStableId)
    {
        return Result.Generate(await userStableService.RemoveUserFromStableAsync(userStableId));
    }
}