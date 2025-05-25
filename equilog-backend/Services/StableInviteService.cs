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

public class StableInviteService(EquilogDbContext context, IMapper mapper) : IStableInviteService
{
    public async Task<ApiResponse<List<UserDto>?>> GetStableInvitesByStableIdAsync(int stableId)
    {
        try
        {
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
    
    public async Task<ApiResponse<Unit>> CreateStableInviteAsync(StableInviteDto stableInviteDto)
    {
        try
        {
            var userStable = await context.UserStables
                .Where(us => us.UserIdFk == stableInviteDto.UserId &&
                             us.StableIdFk == stableInviteDto.StableId)
                .FirstOrDefaultAsync();

            if (userStable != null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.BadRequest,
                    "Error: User is already a member of this stable.");
            
            var stableInvite = new StableInvite
            {
                UserIdFk = stableInviteDto.UserId,
                StableIdFk = stableInviteDto.StableId
            };

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

    public async Task<ApiResponse<Unit>> AcceptStableInviteAsync(StableInviteDto stableInviteDto)
    {
        try
        {
            var stableInvite = await context.StableInvites
                .Where(si =>
                    si.UserIdFk == stableInviteDto.UserId && 
                    si.StableIdFk == stableInviteDto.StableId)
                .FirstOrDefaultAsync();
            
            if (stableInvite == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Stable invite not found.");

            context.StableInvites.Remove(stableInvite);
            await context.SaveChangesAsync();

            var userStable = new UserStable
            {
                UserIdFk = stableInviteDto.UserId,
                StableIdFk = stableInviteDto.StableId,
                Role = 2
            };

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

    public async Task<ApiResponse<Unit>> RefuseStableInviteAsync(StableInviteDto stableInviteDto)
    {
        try
        {
            var stableInvite = await context.StableInvites
                .Where(si =>
                    si.UserIdFk == stableInviteDto.UserId && 
                    si.StableIdFk == stableInviteDto.StableId)
                .FirstOrDefaultAsync();
            
            if (stableInvite == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Stable invite not found.");

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