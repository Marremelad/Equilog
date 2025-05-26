using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.UserStableDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace equilog_backend.Services;

// Service that manages the relationship between users and stables with role-based access control.
// Handles stable memberships, role assignments, and ownership transfer logic for stable management.
public class UserStableService(EquilogDbContext context, IMapper mapper) : IUserStableService
{
    // Retrieves all stable connections for a specific user.
    public async Task<ApiResponse<List<UserStableDto>?>> GetUserStablesByUserIdAsync(int userId)
    {
        try
        {
            // Fetch all stable relationships for the user.
            var userStableDtos = mapper.Map<List<UserStableDto>>(
                await context.UserStables
                    .Where(us => us.UserIdFk == userId)
                    .ToListAsync());

            // Return error if user is not connected to any stables.
            if (userStableDtos == null || userStableDtos.Count == 0)
                return ApiResponse<List<UserStableDto>?>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: User not connected to any stables.");

            return ApiResponse<List<UserStableDto>?>.Success(
                HttpStatusCode.OK,
                userStableDtos,
                "Connections between user and stables fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<UserStableDto>?>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Retrieves all user members of a specific stable with their roles and information.
    public async Task<ApiResponse<List<StableUserDto>?>> GetUserStablesByStableIdAsync(int stableId)
    {
        try
        {
            // Fetch stable members with user information included.
            var userStables = await context.UserStables
                .Where(us => us.StableIdFk == stableId)
                .Include(us => us.User)
                .ToListAsync();

            // Returns an error if no users are found for the stable.
            if (userStables.Count == 0)
                return ApiResponse<List<StableUserDto>?>.Failure(
                    HttpStatusCode.NotFound,
                    $"Error: No users found for stable with ID {stableId}.");

            // Map to DTOs that include user details and roles.
            var stableUserDtos = mapper.Map<List<StableUserDto>>(userStables);

            return ApiResponse<List<StableUserDto>?>.Success(
                HttpStatusCode.OK,
                stableUserDtos,
                "Connection between stable and users fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<StableUserDto>?>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Updates a user's role within a specific stable (owner, admin, or regular user).
    public async Task<ApiResponse<Unit>> UpdateStableUserRoleAsync(int userStableId, int userStableRole)
    {
        try
        {
            // Find the user-stable relationship to update.
            var userStable = await context.UserStables
                .Where(us => us.Id == userStableId)
                .FirstOrDefaultAsync();

            // Returns an error if the relationship doesn't exist.
            if (userStable == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Connection between user and stable not found.");

            // Update the role and save changes.
            userStable.Role = userStableRole;
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK, 
                Unit.Value,
                "Role updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Removes a user from a stable (user-initiated leaving).
    public async Task<ApiResponse<Unit>> LeaveStableAsync(int userId, int stableId)
    {
        try
        {
            // Find the user-stable relationship to remove.
            var userStable = await context.UserStables
                .Where(us => us.UserIdFk == userId && us.StableIdFk == stableId)
                .FirstOrDefaultAsync();
                
            // Returns an error if the relationship doesn't exist.
            if (userStable == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: User not connected to stable.");

            // Remove the relationship and save changes.
            context.UserStables.Remove(userStable);
            await context.SaveChangesAsync();
                
            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "User left stable successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Removes a user from a stable by relationship ID (admin-initiated removal).
    public async Task<ApiResponse<Unit>> RemoveUserFromStableAsync(int userStableId)
    {
        try
        {
            // Find the user-stable relationship to remove.
            var userStable = await context.UserStables
                .Where(us => us.Id == userStableId)
                .FirstOrDefaultAsync();

            // Returns an error if the relationship doesn't exist.
            if (userStable == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Connection between user and stable not found.");
                
            // Remove the relationship and save changes.
            context.Remove(userStable);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.NoContent,
                Unit.Value,
                "User successfully removed from stable.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Creates a new user-stable relationship with an owner role (role 0).
    public async Task<ApiResponse<Unit>> CreateUserStableConnectionAsync(int userId, int stableId)
    {
        try
        {
            // Create the user-stable relationship with owner privileges.
            var userStable = new UserStable
            {
                UserIdFk = userId,
                StableIdFk = stableId,
                Role = 0  // Role 0 = Master admin/Owner with full control.
            };

            // Add the relationship to the database and save.
            context.UserStables.Add(userStable);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.Created,
                Unit.Value,
                "Connection between user and stable was created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Retrieves all stable connections where the user has an owner role (used for ownership transfer).
    public async Task<List<UserStable>> GetConnectionsWithOwnerRole(int userId)
    {
        return await context.UserStables
            .Where(us => us.UserIdFk == userId && us.Role == 0)
            .ToListAsync();
    }

    // Checks if a stable has only one member (used for ownership transfer logic).
    public async Task<bool> HasOnlyOneUser(int stableId)
    {
        var count = await context.UserStables
            .CountAsync(us => us.StableIdFk == stableId);

        return count == 1;
    }

    // Checks if a stable has multiple owners (used for ownership transfer logic).
    public async Task<bool> HasMoreThanOneOwner(UserStable connection)
    {
        var ownerCount = await context.UserStables
            .CountAsync(us => us.StableIdFk == connection.StableIdFk && us.Role == 0);

        return ownerCount >= 2;
    }
    
    // Finds an admin or regular user to promote to an owner when the current owner leaves.
    public async Task<UserStable> FindAdminOrUser(int stableId, int excludeUserId)
    {
        return await context.UserStables
            .Where(us => us.StableIdFk == stableId && 
                         (us.Role == 1 || us.Role == 2) && 
                         us.UserIdFk != excludeUserId)
            .OrderBy(us => us.Role)
            .FirstAsync();
    }
    
    // Promotes a user to an owner role for ownership transfer scenarios.
    public async Task SetRoleToOwner(UserStable connection)
    {
        connection.Role = 0;  // Set to an owner role.
        await context.SaveChangesAsync();
    }
}