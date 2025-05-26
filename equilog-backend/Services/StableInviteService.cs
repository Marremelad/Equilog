using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableInviteDTOs;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

// Service that manages stable invitations sent by stable administrators to potential members.
// Handles the invitation lifecycle from creation to acceptance or refusal.
public class StableInviteService(EquilogDbContext context, IMapper mapper) : IStableInviteService
{
    // Retrieves all users who have been invited to join a specific stable.
    public async Task<ApiResponse<List<UserDto>?>> GetStableInvitesByStableIdAsync(int stableId)
    {
        try
        {
            // Fetch invited users for the stable through the StableInvites junction table.
            var stableInvites = await context.StableInvites
                .Where(si => si.StableIdFk == stableId)
                .Select(si => si.User)
                .ToListAsync();

            return ApiResponse<List<UserDto>>.Success(
                HttpStatusCode.OK,
                mapper.Map<List<UserDto>>(stableInvites),
                "Stable invites fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<UserDto>>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Creates a new invitation for a user to join a stable.
    public async Task<ApiResponse<Unit>> CreateStableInviteAsync(StableInviteDto stableInviteDto)
    {
        try
        {
            // Check if the user is already a member of the stable.
            var userStable = await context.UserStables
                .Where(us => us.UserIdFk == stableInviteDto.UserId &&
                             us.StableIdFk == stableInviteDto.StableId)
                .FirstOrDefaultAsync();

            // Prevent inviting users who are already members.
            if (userStable != null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.BadRequest,
                    "Error: User is already a member of this stable.");
            
            // Creates a stable invitation relationship.
            var stableInvite = new StableInvite
            {
                UserIdFk = stableInviteDto.UserId,
                StableIdFk = stableInviteDto.StableId
            };

            // Add the invitation to the database.
            context.StableInvites.Add(stableInvite);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.Created,
                Unit.Value,
                "Stable invite created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Processes acceptance of a stable invitation by converting it to membership.
    public async Task<ApiResponse<Unit>> AcceptStableInviteAsync(StableInviteDto stableInviteDto)
    {
        try
        {
            // Find the existing invitation.
            var stableInvite = await context.StableInvites
                .Where(si =>
                    si.UserIdFk == stableInviteDto.UserId && 
                    si.StableIdFk == stableInviteDto.StableId)
                .FirstOrDefaultAsync();
            
            // Returns an error if the invitation doesn't exist.
            if (stableInvite == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Stable invite not found.");

            // Remove the invitation since it's being accepted.
            context.StableInvites.Remove(stableInvite);
            await context.SaveChangesAsync();

            // Create user-stable membership with a regular user role (role 2).
            var userStable = new UserStable
            {
                UserIdFk = stableInviteDto.UserId,
                StableIdFk = stableInviteDto.StableId,
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

    // Processes refusal of a stable invitation by removing it without creating membership.
    public async Task<ApiResponse<Unit>> RefuseStableInviteAsync(StableInviteDto stableInviteDto)
    {
        try
        {
            // Find the existing invitation.
            var stableInvite = await context.StableInvites
                .Where(si =>
                    si.UserIdFk == stableInviteDto.UserId && 
                    si.StableIdFk == stableInviteDto.StableId)
                .FirstOrDefaultAsync();
            
            // Returns an error if the invitation doesn't exist.
            if (stableInvite == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Stable invite not found.");

            // Remove the invitation without creating membership.
            context.StableInvites.Remove(stableInvite);
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