using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableJoinRequestDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;

namespace equilog_backend.Services;

public class StableJoinRequestService(EquilogDbContext context, IMapper mapper) : IStableJoinRequestService
{
    public async Task<ApiResponse<Unit>> CreateStableJoinRequest(StableJoinRequestDto stableJoinRequestDto)
    {
        try
        {
            var stableJoinRequest = new StableJoinRequest
            {
                UserIdFk = stableJoinRequestDto.UserId,
                StableIdFk = stableJoinRequestDto.StableId
            };

            context.StableJoinRequests.Add(stableJoinRequest);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(HttpStatusCode.Created,
                Unit.Value,
                "Stable join request created successfully");

        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}