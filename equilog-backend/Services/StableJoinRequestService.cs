using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableJoinRequestDTOs;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class StableJoinRequestService(EquilogDbContext context, IMapper mapper) : IStableJoinRequestService
{
    public async Task<ApiResponse<List<UserDto>?>> GetStableJoinRequestsByStableAsync(int stableId)
    {
        try
        {
            var stableJoinRequests = await context.StableJoinRequests
                .Where(sjr => sjr.StableIdFk == stableId)
                .Select(srj => srj.User)
                .ToListAsync();

            return ApiResponse<List<UserDto>>.Success(HttpStatusCode.OK,
                mapper.Map<List<UserDto>>(stableJoinRequests),
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<UserDto>>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    public async Task<ApiResponse<Unit>> CreateStableJoinRequestAsync(StableJoinRequestDto stableJoinRequestDto)
    {
        try
        {
            var userStable = await context.UserStables
                .Where(us => us.UserIdFk == stableJoinRequestDto.UserId &&
                             us.StableIdFk == stableJoinRequestDto.StableId)
                .FirstOrDefaultAsync();

            if (userStable != null)
            {
                return ApiResponse<Unit>.Failure(HttpStatusCode.BadRequest,
                    "Error: User is already a member of this stable");
            }
            
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

    public async Task<ApiResponse<Unit>> AcceptStableJoinRequestAsync(StableJoinRequestDto stableJoinRequestDto)
    {
        try
        {
            var stableJoinRequest = await context.StableJoinRequests
                .Where(sjr =>
                    sjr.UserIdFk == stableJoinRequestDto.UserId && 
                    sjr.StableIdFk == stableJoinRequestDto.StableId)
                .FirstOrDefaultAsync();
            
            if (stableJoinRequest == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Error: Stable join request not found");

            context.StableJoinRequests.Remove(stableJoinRequest);
            await context.SaveChangesAsync();

            var userStable = new UserStable
            {
                UserIdFk = stableJoinRequestDto.UserId,
                StableIdFk = stableJoinRequestDto.StableId,
                Role = 2
            };

            context.UserStables.Add(userStable);
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                "User was accepted into stable successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> DenyStableJoinRequestAsync(StableJoinRequestDto stableJoinRequestDto)
    {
        try
        {
            var stableJoinRequest = await context.StableJoinRequests
                .Where(sjr =>
                    sjr.UserIdFk == stableJoinRequestDto.UserId && 
                    sjr.StableIdFk == stableJoinRequestDto.StableId)
                .FirstOrDefaultAsync();
            
            if (stableJoinRequest == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Error: Stable join request not found");

            context.StableJoinRequests.Remove(stableJoinRequest);
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                "User was not accepted into stable successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}