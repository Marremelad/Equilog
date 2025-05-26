using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using System.Net;

namespace equilog_backend.Services;

// Service that manages the relationship between users and horses.
// Handles horse ownership assignments and role-based access to horses within the system.
public class UserHorseService(EquilogDbContext context) : IUserHorseService
{
    // Creates a relationship between a user and a horse (establishes horse ownership with an owner role).
    public async Task<ApiResponse<Unit>> CreateUserHorseConnectionAsync(int userId, int horseId)
    {
        try
        {
            // Create the user-horse relationship entity with owner privileges (role 0).
            var userHorse = new UserHorse
            {
                UserIdFk = userId,
                HorseIdFk = horseId,
                UserRole = 0  // Role 0 = Owner with full access to the horse.
            };
            
            // Add the relationship to the database and save.
            context.UserHorses.Add(userHorse);
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(
                HttpStatusCode.Created,
                Unit.Value,
                "Connection between user and horse was created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}