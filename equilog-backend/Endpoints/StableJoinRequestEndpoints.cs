using equilog_backend.Common;
using equilog_backend.DTOs.StableJoinRequestDTOs;
using equilog_backend.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace equilog_backend.Endpoints;

public class StableJoinRequestEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/api/create-stable-join-request", CreateStableJoinRequest)
            .WithName("CreateStableJoinRequest");

        app.MapPost("/api/accept-stable-join-request", AcceptStableJoinRequest)
            .WithName("AcceptStableJoinRequest");

        app.MapPost("/api/deny-stable-join-request", DenyStableJoinRequest)
            .WithName("DenyStableJoinRequest");
    }

    private static async Task<IResult> CreateStableJoinRequest(
        IStableJoinRequestService stableJoinRequestService,
        StableJoinRequestDto stableJoinRequestDto)
    {
        return Result.Generate(await stableJoinRequestService.CreateStableJoinRequestAsync(stableJoinRequestDto));
    }

    private static async Task<IResult> AcceptStableJoinRequest(
        IStableJoinRequestService stableJoinRequestService,
        StableJoinRequestDto stableJoinRequestDto)
    {
        return Result.Generate(await stableJoinRequestService.AcceptStableJoinRequestAsync(stableJoinRequestDto));
    }

    private static async Task<IResult> DenyStableJoinRequest(
        IStableJoinRequestService stableJoinRequestService,
        StableJoinRequestDto stableJoinRequestDto)
    {
        return Result.Generate(await stableJoinRequestService.DenyStableJoinRequestAsync(stableJoinRequestDto));
    }
}