using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableInviteDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class StableInviteService(EquilogDbContext context, IMapper mapper) : IStableInviteService
{
    public async Task<ApiResponse<Unit>> CreateStableInviteAsync(StableInviteDto stableInviteDto)
    {
        try
        {
            var userStable = await context.UserStables
                .Where(us => us.UserIdFk == stableInviteDto.UserId &&
                             us.StableIdFk == stableInviteDto.StableId)
                .FirstOrDefaultAsync();

            if (userStable != null)
            {
                return ApiResponse<Unit>.Failure(HttpStatusCode.BadRequest,
                    "Error: User is already a member of this stable");
            }
            
            var stableInvite = new StableInvite
            {
                UserIdFk = stableInviteDto.UserId,
                StableIdFk = stableInviteDto.StableId
            };

            context.StableInvites.Add(stableInvite);
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