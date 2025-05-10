using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.DTOs.UserStableDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace equilog_backend.Services
{
    public class UserStableService(EquilogDbContext context, IMapper mapper) : IUserStableService
    {
        public async Task<ApiResponse<List<UserStableDto>?>> GetUserStablesAsync(int userId)
        {
            try
            {
                var userStableDtos = mapper.Map<List<UserStableDto>>(
                    await context.UserStables
                    .Where(us => us.UserIdFk == userId)
                    .ToListAsync());

                if (userStableDtos == null || userStableDtos.Count == 0)
                    return ApiResponse<List<UserStableDto>?>.Failure(HttpStatusCode.NotFound,
                        "Error: User not connected to any stables");

                return ApiResponse<List<UserStableDto>?>.Success(HttpStatusCode.OK, 
                    userStableDtos, 
                    null);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<UserStableDto>?>.Failure(HttpStatusCode.InternalServerError, 
                    ex.Message);
            }
        }

        public async Task<ApiResponse<List<StableUserDto>?>> GetStableUsersAsync(int stableId)
        {
            try
            {
                var userStables = await context.UserStables
                    .Where(us => us.StableIdFk == stableId)
                    .Include(us => us.User)
                    .ToListAsync();

                if (userStables.Count == 0)
                {
                    return ApiResponse<List<StableUserDto>?>.Failure(
                        HttpStatusCode.NotFound,
                        $"Error: No users found for stable with ID {stableId}");
                }

                var stableUserDtos = mapper.Map<List<StableUserDto>>(userStables);

                return ApiResponse<List<StableUserDto>?>.Success(HttpStatusCode.OK,stableUserDtos,null);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<StableUserDto>?>.Failure(
                    HttpStatusCode.InternalServerError,
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
                {
                    return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    $"userStable with ID: {userStableId} not found");
                }

                userStable.Role = userStableRole;
                await context.SaveChangesAsync();

                return ApiResponse<Unit>.Success(HttpStatusCode.OK, Unit.Value, "Role updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }

        public async Task<ApiResponse<Unit>> CreateUserStableConnectionOnStableCreation(int userId, int stableId)
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
                    null);

            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }
    }
}
