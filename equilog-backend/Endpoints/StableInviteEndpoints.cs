using equilog_backend.Common;
using equilog_backend.DTOs.StableInviteDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class StableInviteEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/api/get-stable-invite-by-stable/{id:int}", GetStableInviteByStableId)
            .WithName("GetStableInviteByStableId");

        app.MapPost("/api/create-stable-invite", CreateStableInvite)
            .WithName("CreateStableInvite");

        app.MapPost("/api/accept-stable-invite", AcceptStableInvite)
            .WithName("AcceptStableInvite");

        app.MapPost("/api/refuse-stable-invite", RefuseStableInvite)
            .WithName("RefuseStableInvite");
    }

    private static async Task<IResult> GetStableInviteByStableId(
        IStableInviteService stableInviteService,
        int id)
    {
        return Result.Generate(await stableInviteService.GetStableInvitesByStableIdAsync(id));
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