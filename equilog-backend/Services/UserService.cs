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

public class UserService(EquilogDbContext context, IMapper mapper) : IUserService
{
    public async Task<ApiResponse<List<UserDto>?>> GetUsersAsync()
    {
        try
        {
            var userDtos = mapper.Map<List<UserDto>>(await context.Users.ToListAsync());

            return ApiResponse<List<UserDto>>.Success(HttpStatusCode.OK, userDtos, null);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<UserDto>>.Failure(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<ApiResponse<UserDto?>> GetUserAsync(int userId)
    {
        try
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                return ApiResponse<UserDto>.Failure(HttpStatusCode.NotFound,
                    "Error: User not found");

            return ApiResponse<UserDto>.Success(HttpStatusCode.OK,
                mapper.Map<UserDto>(user),
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<UserProfileDto?>> GetUserProfileAsync(int userId, int stableId)
    {
        try
        {
            var userExists = await context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
            if (userExists == null)
            {
                return ApiResponse<UserProfileDto>.Failure(HttpStatusCode.NotFound,
                    "Error: User not found");
            }

            var userStableRoleDto = mapper.Map<UserStableRoleDto>(
                await context.UserStables.FirstOrDefaultAsync(us => us.UserIdFk == userId && us.StableIdFk == stableId)
            );

            if (userStableRoleDto == null)
            {
                return ApiResponse<UserProfileDto>.Failure(HttpStatusCode.NotFound,
                    "Error: User stable connection not found.");
            }

            var userHorseRoleDtos = await context.UserHorses
                .Where(uh => uh.UserIdFk == userId &&
                             context.StableHorses.Any(sh => sh.HorseIdFk == uh.HorseIdFk && sh.StableIdFk == stableId))
                .ProjectTo<HorseWithUserHorseRoleDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            var userProfileDto = new UserProfileDto
            {
                UserStableRole = userStableRoleDto,
                UserHorseRoles = userHorseRoleDtos
            };

            return ApiResponse<UserProfileDto>.Success(HttpStatusCode.OK,
                userProfileDto,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserProfileDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> UpdateUserAsync(UserUpdateDto userUpdateDto)
    {
        try
        {
            var user = await context.Users
                .Where(u => u.Id == userUpdateDto.Id)
                .FirstOrDefaultAsync();

            if (user == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Error: User not found");

            mapper.Map(userUpdateDto, user);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                "User information updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> DeleteUserAsync(int userId)
    {
        try
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Error: User not found");

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(HttpStatusCode.NoContent,
                Unit.Value,
                $"User with id '{userId}' was deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    public Task<ApiResponse<Unit>> SetProfilePictureAsync(int userId, string uri)
    {
        throw new NotImplementedException();
    }
}