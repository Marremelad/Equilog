using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services
{
    public class UserService(EquilogDbContext context, IMapper mapper)
    {
        public async Task<ApiResponse<List<UserDto>?>> GetUsers()
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

        public async Task<ApiResponse<UserDto?>> GetUser(int id)
        {
            try
            {
                var user = await context.Users
                    .Where(u => u.Id == id)
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

        public async Task<ApiResponse<UserDto?>> CreateUser(UserCreateDto newUser)
        {
            try
            {
                var user = mapper.Map<User>(newUser);

                context.Users.Add(user);
                await context.SaveChangesAsync();

                return ApiResponse<UserDto>.Success(HttpStatusCode.Created,
                    mapper.Map<UserDto>(user),
                    "User created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }

        public async Task<ApiResponse<UserDto?>> UpdateUser(UserUpdateDto updatedUser)
        {
            try
            {
                var user = await context.Users
                    .Where(u => u.Id == updatedUser.Id)
                    .FirstOrDefaultAsync();

                if (user == null)
                    return ApiResponse<UserDto>.Failure(HttpStatusCode.NotFound,
                    "Error: User not found");

                mapper.Map(updatedUser, user);
                await context.SaveChangesAsync();

                return ApiResponse<UserDto>.Success(HttpStatusCode.OK,
                    mapper.Map<UserDto>(user),
                    "User information updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }

        public async Task<ApiResponse<UserDto?>> DeleteUser(int id)
        {
            try
            {
                var user = await context.Users
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();

                if (user == null)
                    return ApiResponse<UserDto>.Failure(HttpStatusCode.NotFound,
                    "Error: User not found");

                context.Users.Remove(user);
                await context.SaveChangesAsync();

                return ApiResponse<UserDto>.Success(HttpStatusCode.NoContent,
                    null,
                    $"User with id '{id}' was deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }
    }
}
