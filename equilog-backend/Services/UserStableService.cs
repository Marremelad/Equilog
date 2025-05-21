using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.UserStableDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace equilog_backend.Services;

public class UserStableService(EquilogDbContext context, IMapper mapper) : IUserStableService
{
    public async Task<ApiResponse<List<UserStableDto>?>> GetUserStablesByUserIdAsync(int userId)
    {
        try
        {
            var userStableDtos = mapper.Map<List<UserStableDto>>(
                await context.UserStables
                    .Where(us => us.UserIdFk == userId)
                    .ToListAsync());

            if (userStableDtos == null || userStableDtos.Count == 0)
                return ApiResponse<List<UserStableDto>?>.Failure(HttpStatusCode.NotFound,
                    "Error: User not connected to any stables.");

            return ApiResponse<List<UserStableDto>?>.Success(HttpStatusCode.OK,
                userStableDtos,
                "Connections between user and stables fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<UserStableDto>?>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<List<StableUserDto>?>> GetUserStablesByStableIdAsync(int stableId)
    {
        try
        {
            var userStables = await context.UserStables
                .Where(us => us.StableIdFk == stableId)
                .Include(us => us.User)
                .ToListAsync();

            if (userStables.Count == 0)
                return ApiResponse<List<StableUserDto>?>.Failure(HttpStatusCode.NotFound,
                    $"Error: No users found for stable with ID {stableId}.");

            var stableUserDtos = mapper.Map<List<StableUserDto>>(userStables);

            return ApiResponse<List<StableUserDto>?>.Success(HttpStatusCode.OK,
                stableUserDtos,
                "Connection between stable and users fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<StableUserDto>?>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> UpdateStableUserRoleAsync(int userStableId, int userStableRole)
    {
        try
        {
            var userStable = await context.UserStables
                .Where(us => us.Id == userStableId)
                .FirstOrDefaultAsync();

            if (userStable == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Connection between user and stable not found.");

            userStable.Role = userStableRole;
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(HttpStatusCode.OK, 
                Unit.Value,
                "Role updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    public async Task<ApiResponse<Unit>> LeaveStableAsync(int userId, int stableId)
    {
        try
        {
            var userStable = await context.UserStables
                .Where(us => us.UserIdFk == userId && us.StableIdFk == stableId)
                .FirstOrDefaultAsync();
                
            if (userStable == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Error: User not connected to stable.");

            context.UserStables.Remove(userStable);
            await context.SaveChangesAsync();
                
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                "User left stable successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    public async Task<ApiResponse<Unit>> RemoveUserFromStableAsync(int userStableId)
    {
        try
        {
            var userStable = await context.UserStables
                .Where(us => us.Id == userStableId)
                .FirstOrDefaultAsync();

            if (userStable == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Connection between user and stable not found.");
                
            context.Remove(userStable);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(HttpStatusCode.NoContent,
                Unit.Value,
                "User successfully removed from stable.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> CreateUserStableConnectionAsync(int userId, int stableId)
    {
        try
        {
            var userStable = new UserStable
            {
                UserIdFk = userId,
                StableIdFk = stableId,
                Role = 0
            };

            context.UserStables.Add(userStable);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(HttpStatusCode.Created,
                Unit.Value,
                "Connection between user and stable was created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    public async Task<List<UserStable>> GetConnectionsWithOwnerRole(int userId)
    {
        return await context.UserStables
            .Where(us => us.UserIdFk == userId && us.Role == 0)
            .ToListAsync();
    }

    public async Task<bool> HasOnlyOneUser(int stableId)
    {
        var count = await context.UserStables
            .CountAsync(us => us.StableIdFk == stableId);

        return count == 1;
    }

    public async Task<bool> HasMoreThanOneOwner(UserStable connection)
    {
        var ownerCount = await context.UserStables
            .CountAsync(us => us.StableIdFk == connection.StableIdFk && us.Role == 0);

        return ownerCount >= 2;
    }

    public async Task<UserStable> FindAdminOrUser(int stableId, int excludeUserId)
    {
        return await context.UserStables
            .Where(us => us.StableIdFk == stableId && 
                         (us.Role == 1 || us.Role == 2) && 
                         us.UserIdFk != excludeUserId)
            .OrderBy(us => us.Role)
            .FirstAsync();
    }

    public async Task SetRoleToOwner(UserStable connection)
    {
        connection.Role = 0;
        await context.SaveChangesAsync();
    }
}