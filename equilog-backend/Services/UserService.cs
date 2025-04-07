using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services
{
    public class UserService(EquilogDbContext context, IMapper mapper)
    {
        public async Task<ApiResponse<List<UserDto>?>> GetAllUsers()
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
    }
}
