using AutoMapper;
using AutoMapper.QueryableExtensions;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.DTOs.UserHorseDTOs;
using equilog_backend.DTOs.UserStableDTOs;
using equilog_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace equilog_backend.Services;

// Service that manages user entities and their relationships within the stable management system.
// Handles user CRUD operations, profile management, and complex user-stable-horse relationship queries.
public class UserService(EquilogDbContext context, IMapper mapper) : IUserService
{
    // Retrieves all users in the system (used for testing and administrative purposes).
    public async Task<ApiResponse<List<UserDto>?>> GetUsersAsync()
    {
        try
        {
            // Fetch all users and map to DTOs.
            var userDtos = mapper.Map<List<UserDto>>(await context.Users.ToListAsync());

            return ApiResponse<List<UserDto>>.Success(
                HttpStatusCode.OK,
                userDtos, 
                "Users fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<UserDto>>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Retrieves a specific user by their ID.
    public async Task<ApiResponse<UserDto?>> GetUserAsync(int userId)
    {
        try
        {
            // Find the user by ID.
            var user = await context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            // Returns an error if the user doesn't exist.
            if (user == null)
                return ApiResponse<UserDto>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: User not found");

            return ApiResponse<UserDto>.Success(
                HttpStatusCode.OK,
                mapper.Map<UserDto>(user),
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Retrieves comprehensive user profile information within the context of a specific stable.
    public async Task<ApiResponse<UserProfileDto?>> GetUserProfileAsync(int userId, int stableId)
    {
        try
        {
            // Verify the user exists in the system.
            var userExists = await context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
            
            if (userExists == null)
            {
                return ApiResponse<UserProfileDto>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: User not found.");
            }
            
            // Get the user's role within the specified stable.
            var userStableRoleDto = mapper.Map<UserStableRoleDto>(
                await context.UserStables
                    .FirstOrDefaultAsync(
                        us => us.UserIdFk == userId && us.StableIdFk == stableId)
            );
            
            // Returns an error if the user is not a member of the stable.
            if (userStableRoleDto == null)
            {
                return ApiResponse<UserProfileDto>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: User stable connection not found.");
            }
            
            // Get all horses owned by the user that are also housed in the specified stable.
            var userHorseRoleDtos = await context.UserHorses
                .Where(uh => uh.UserIdFk == userId &&
                             context.StableHorses.Any(sh => sh.HorseIdFk == uh.HorseIdFk && sh.StableIdFk == stableId))
                .ProjectTo<HorseWithUserHorseRoleDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            
            // Build a comprehensive user profile with stable role and horse relationships.
            var userProfileDto = new UserProfileDto
            {
                UserStableRole = userStableRoleDto,
                UserHorseRoles = userHorseRoleDtos
            };

            return ApiResponse<UserProfileDto>.Success(
                HttpStatusCode.OK,
                userProfileDto,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserProfileDto>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Updates an existing user with new information.
    public async Task<ApiResponse<Unit>> UpdateUserAsync(UserUpdateDto userUpdateDto)
    {
        try
        {
            // Find the existing user to update.
            var user = await context.Users
                .Where(u => u.Id == userUpdateDto.Id)
                .FirstOrDefaultAsync();

            // Returns an error if the user doesn't exist.
            if (user == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: User not found");

            // Apply updates from DTO to an existing entity and save changes.
            mapper.Map(userUpdateDto, user);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "User information updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Removes a user from the system along with all their relationships.
    public async Task<ApiResponse<Unit>> DeleteUserAsync(int userId)
    {
        try
        {
            // Find the user to delete.
            var user = await context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            // Returns an error if the user doesn't exist.
            if (user == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: User not found");

            // Remove the user (cascade delete will handle relationship cleanup).
            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.NoContent,
                Unit.Value,
                $"User with id '{userId}' was deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Updates a user's profile picture by setting the blob storage reference.
    public async Task<ApiResponse<Unit>> SetProfilePictureAsync(int userId, string blobName)
    {
        try
        {
            // Find the user to update.
            var user = await context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
                
            // Returns an error if the user doesn't exist.
            if (user == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: User not found.");

            // Set the profile picture blob reference and save changes.
            user.ProfilePicture = blobName;
            await context.SaveChangesAsync();
                
            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                $"profile picture for user '{userId}' was set successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}