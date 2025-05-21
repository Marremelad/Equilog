using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class UserStableEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/api/user-stables/user/{id:int}", GetUserStablesByUserId)
            .WithName("GetUserStablesByUserId");

        app.MapGet("/api/user-stables/stable/{id:int}", GetUserStablesByStableId)
            .WithName("GetUserStablesByStableId");

        app.MapPut("/api/user-stables/stable-user/{id:int}", UpdateStableUserRole)
            .WithName("UpdateStableUserRole");
            
        app.MapDelete("/api/user-stables/{userStableId:int}", RemoveUserFromStable)
            .WithName("RemoveUserFromStable");
            
        // -- Endpoints for compositions --

        app.MapDelete("/api/user-stables/leave/{userId:int}/{stableId:int}", LeaveStableComposition)
            .WithName("LeaveStableComposition");
    }
        
    private static async Task<IResult> GetUserStablesByUserId(
        IUserStableService userStableService,
        int id)
    {
        return Result.Generate(await userStableService.GetUserStablesByUserIdAsync(id));
    }

    private static async Task<IResult> GetUserStablesByStableId(
        IUserStableService userStableService,
        int id)
    {
        return Result.Generate(await userStableService.GetUserStablesByStableIdAsync(id));
    }

    private static async Task<IResult> UpdateStableUserRole(
        IUserStableService userStableService,
        int id,
        int userStableRole)
    {
        return Result.Generate(await userStableService.UpdateStableUserRoleAsync(id, userStableRole));
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