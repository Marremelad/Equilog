using equilog_backend.Common;
using equilog_backend.DTOs.StableJoinRequestDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class StableJoinRequestEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get join-requests for a specific stable.
        app.MapGet("/api/get-stable-join-request-by-stable/{stableId:int}", GetStableJoinRequestByStableId) // "/api/stables/{stableId:int}/join-requests"
            .RequireAuthorization()
            .WithName("GetStableJoinRequestByStableId");
        
        // Get join-requests sent by a specific user.
        app.MapGet("/api/get-stable-join-requests-by-user/{userId:int}", GetStableJoinRequestByUserId) // "/api/users/{userId:int}/join-requests"
            .RequireAuthorization()
            .WithName("GetStableJoinRequestByUserId");
        
        // Create join-request.
        app.MapPost("/api/create-stable-join-request", CreateStableJoinRequest) // "/api/stable-join-requests"
            .RequireAuthorization()
            .WithName("CreateStableJoinRequest");

        // Accept join-request.
        app.MapPost("/api/accept-stable-join-request", AcceptStableJoinRequest) // "/api/stable-join-requests/{requestId:int}/accept"
            .RequireAuthorization()
            .WithName("AcceptStableJoinRequest");

        // Refuse join-request.
        app.MapPost("/api/refuse-stable-join-request", RefuseStableJoinRequest) // "/api/stable-join-requests/{requestId:int}/refuse"
            .RequireAuthorization()
            .WithName("RefuseStableJoinRequest");
    }

    private static async Task<IResult> GetStableJoinRequestByStableId(
        IStableJoinRequestService stableJoinRequestService,
        int stableId)
    {
        return Result.Generate(await stableJoinRequestService.GetStableJoinRequestsByStableIdAsync(stableId));
    }
    
    private static async Task<IResult> GetStableJoinRequestByUserId(
        IStableJoinRequestService stableJoinRequestService,
        int userId)
    {
        return Result.Generate(await stableJoinRequestService.GetStableJoinRequestsByUserIdAsync(userId));
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

    private static async Task<IResult> RefuseStableJoinRequest(
        IStableJoinRequestService stableJoinRequestService,
        StableJoinRequestDto stableJoinRequestDto)
    {
        return Result.Generate(await stableJoinRequestService.RefuseStableJoinRequestAsync(stableJoinRequestDto));
    }
}