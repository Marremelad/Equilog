using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services
{
    public class UserService(EquilogDbContext context, IMapper mapper)
    {
        public async Task<Response<List<UserDto>?>> GetAllUsers()
        {
            try
            {
                var userDtos = mapper.Map<List<UserDto>>(await context.Users.ToListAsync());

                return Response<List<UserDto>>.Success(userDtos);
            }
            catch (Exception ex)
            {
                return Response<List<UserDto>>.Failure(ex.Message);
            }
        }
    }
}
