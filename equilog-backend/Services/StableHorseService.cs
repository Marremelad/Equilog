using System.Net;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class StableHorseService(EquilogDbContext context) : IStableHorseService
{
    public async Task<ApiResponse<Unit>> CreateStableHorseConnectionAsync(int stableId, int horseId)
    {
        try
        {
            var stableHorse = new StableHorse
            {
                StableIdFk = stableId,
                HorseIdFk = horseId
            };

            context.StableHorses.Add(stableHorse);
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

    public async Task<ApiResponse<Unit>> RemoveHorseFromStable(int stableHorseId)
    {
        try
        {
            var stableHorse = await context.StableHorses
                .Where(sh => sh.Id == stableHorseId)
                .FirstOrDefaultAsync();
            
            if (stableHorse == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotAcceptable,
                    "Error: Relation entity not found.");
            
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
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