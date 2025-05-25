using equilog_backend.Common;
using equilog_backend.DTOs.StableInviteDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class StableInviteEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get invitation sent by stable.
        app.MapGet("/api/get-stable-invite-by-stable/{stableId:int}", GetStableInviteByStableId) // "/api/stables/{stableId:int}/invites"
            .WithName("GetStableInviteByStableId");

        // Create invitation to stable.
        app.MapPost("/api/create-stable-invite", CreateStableInvite) // "/api/stable-invites"
            .WithName("CreateStableInvite");

        // Accept invitation to stable.
        app.MapPost("/api/accept-stable-invite", AcceptStableInvite) // "/api/stable-invites/{inviteId:int}/accept"
            .WithName("AcceptStableInvite");

        // Refuse invitation to stable.
        app.MapPost("/api/refuse-stable-invite", RefuseStableInvite) // "/api/stable-invites/{inviteId:int}/refuse"
            .WithName("RefuseStableInvite");
    }

    private static async Task<IResult> GetStableInviteByStableId(
        IStableInviteService stableInviteService,
        int stableId)
    {
        return Result.Generate(await stableInviteService.GetStableInvitesByStableIdAsync(stableId));
    }

    private static async Task<IResult> CreateStableInvite(
        IStableInviteService stableInviteService,
        StableInviteDto stableInviteDto)
    {
        return Result.Generate(await stableInviteService.CreateStableInviteAsync(stableInviteDto));
    }

    private static async Task<IResult> AcceptStableInvite(
        IStableInviteService stableInviteService,
        StableInviteDto stableInviteDto)
    {
        return Result.Generate(await stableInviteService.AcceptStableInviteAsync(stableInviteDto));
    }

    private static async Task<IResult> RefuseStableInvite(
        IStableInviteService stableInviteService,
        StableInviteDto stableInviteDto)
    {
        return Result.Generate(await stableInviteService.RefuseStableInviteAsync(stableInviteDto));
    }
}