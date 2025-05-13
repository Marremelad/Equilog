using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using System.Net;

namespace equilog_backend.Services
{
    public class UserHorseService(EquilogDbContext context) : IUserHorseService
    {
        public async Task<ApiResponse<Unit>> CreateUserHorseConnectionAsync(int userId, int horseId) 
        {
            try
            {
                var userHorse = new UserHorse
                {
                    UserIdFk = userId,
                    HorseIdFk = horseId,
                    UserRole = 0
                };

                context.UserHorses.Add(userHorse);
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
