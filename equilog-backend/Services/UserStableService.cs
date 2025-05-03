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

                if (userStableDtos == null || !userStableDtos.Any())
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
    }
}
