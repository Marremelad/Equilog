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
    }

    private static async Task<IResult> CreateStableJoinRequest(
        IStableJoinRequestService stableJoinRequestService,
        StableJoinRequestDto stableJoinRequestDto)
    {
        return Result.Generate(await stableJoinRequestService.CreateStableJoinRequest(stableJoinRequestDto));
    }

    private static async Task<IResult> AcceptStableJoinRequest(
        IStableJoinRequestService stableJoinRequestService,
        StableJoinRequestDto stableJoinRequestDto)
    {
        return Result.Generate(await stableJoinRequestService.AcceptStableJoinRequest(stableJoinRequestDto));
    }
}