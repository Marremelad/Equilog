using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services
{
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
    }
}
