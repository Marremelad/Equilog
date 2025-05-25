using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.DTOs.StableJoinRequestDTOs;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

// Service that manages join requests initiated by users who want to join specific stables.
// Handles the request lifecycle from creation by users to approval or rejection by stable administrators.
public class StableJoinRequestService(EquilogDbContext context, IMapper mapper) : IStableJoinRequestService
{
    // Retrieves all users who have requested to join a specific stable.
    public async Task<ApiResponse<List<UserDto>?>> GetStableJoinRequestsByStableIdAsync(int stableId)
    {
        try
        {
            // Fetch users who have requested to join the stable through the junction table.
            var stableJoinRequests = await context.StableJoinRequests
                .Where(sjr => sjr.StableIdFk == stableId)
                .Select(sjr => sjr.User)
                .ToListAsync();

            return ApiResponse<List<UserDto>>.Success(
                HttpStatusCode.OK,
                mapper.Map<List<UserDto>>(stableJoinRequests),
                "Stable join requests was fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<UserDto>>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Retrieves all stables that a specific user has requested to join.
    public async Task<ApiResponse<List<StableDto>?>> GetStableJoinRequestsByUserIdAsync(int userId)
    {
        try
        {
            // Fetch stables that the user has requested to join.
            var stableJoinRequests = await context.StableJoinRequests
                .Where(sjr => sjr.UserIdFk == userId)
                .Select(sjr => sjr.Stable)
                .ToListAsync();

            return ApiResponse<List<StableDto>>.Success(
                HttpStatusCode.OK,
                mapper.Map<List<StableDto>>(stableJoinRequests),
                "Stable join requests fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<StableDto>>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Creates a new join request for a user to join a specific stable.
    public async Task<ApiResponse<Unit>> CreateStableJoinRequestAsync(StableJoinRequestDto stableJoinRequestDto)
    {
        try
        {
            // Check if the user is already a member of the stable.
            var userStable = await context.UserStables
                .Where(us => us.UserIdFk == stableJoinRequestDto.UserId &&
                             us.StableIdFk == stableJoinRequestDto.StableId)
                .FirstOrDefaultAsync();

            // Prevent creating requests for users who are already members.
            if (userStable != null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.BadRequest,
                    "User is already a member of this stable.");

            // Check if the user has already sent a join request for this stable.
            var storedStableJoinRequest = await context.StableJoinRequests
                .Where(sjr => sjr.UserIdFk == stableJoinRequestDto.UserId &&
                              sjr.StableIdFk == stableJoinRequestDto.StableId)
                .FirstOrDefaultAsync();
            
            // Prevent duplicate join requests.
            if (storedStableJoinRequest != null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.BadRequest,
                    "User har already sent a join request to this stable.");
            
            // Create a stable join request relationship.
            var stableJoinRequest = new StableJoinRequest
            {
                UserIdFk = stableJoinRequestDto.UserId,
                StableIdFk = stableJoinRequestDto.StableId
            };

            // Add the join request to the database.
            context.StableJoinRequests.Add(stableJoinRequest);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.Created,
                Unit.Value,
                "Stable join request created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Processes acceptance of a join request by converting it to stable membership.
    public async Task<ApiResponse<Unit>> AcceptStableJoinRequestAsync(StableJoinRequestDto stableJoinRequestDto)
    {
        try
        {
            // Find the existing join request.
            var stableJoinRequest = await context.StableJoinRequests
                .Where(sjr =>
                    sjr.UserIdFk == stableJoinRequestDto.UserId && 
                    sjr.StableIdFk == stableJoinRequestDto.StableId)
                .FirstOrDefaultAsync();
            
            // Returns an error if the join request doesn't exist.
            if (stableJoinRequest == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Stable join request not found.");

            // Remove the join request since it's being accepted.
            context.StableJoinRequests.Remove(stableJoinRequest);
            await context.SaveChangesAsync();

            // Create user-stable membership with a regular user role (role 2).
            var userStable = new UserStable
            {
                UserIdFk = stableJoinRequestDto.UserId,
                StableIdFk = stableJoinRequestDto.StableId,
                Role = 2
            };

            // Add the new membership to the database.
            context.UserStables.Add(userStable);
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "User was accepted into stable successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Processes refusal of a join request by removing it without creating membership.
    public async Task<ApiResponse<Unit>> RefuseStableJoinRequestAsync(StableJoinRequestDto stableJoinRequestDto)
    {
        try
        {
            // Find the existing join request.
            var stableJoinRequest = await context.StableJoinRequests
                .Where(sjr =>
                    sjr.UserIdFk == stableJoinRequestDto.UserId && 
                    sjr.StableIdFk == stableJoinRequestDto.StableId)
                .FirstOrDefaultAsync();
            
            // Returns an error if join the request doesn't exist.
            if (stableJoinRequest == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Stable join request not found.");

            // Remove the join request without creating membership.
            context.StableJoinRequests.Remove(stableJoinRequest);
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "User was not accepted into stable successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}