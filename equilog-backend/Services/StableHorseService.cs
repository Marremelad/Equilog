using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableHorseDTOs;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class StableHorseService(EquilogDbContext context, IMapper mapper) : IStableHorseService
{
    public async Task<ApiResponse<List<StableHorseDto>?>> GetStableHorsesAsync(int stableId)
    {
        try
        {
            var stableHorseDtos = mapper.Map<List<StableHorseDto>>(
                await context.StableHorses
                .Where(sh => sh.StableIdFk == stableId)
                .ToListAsync()); 
            
            if (stableHorseDtos == null || stableHorseDtos.Count == 0 )
                return ApiResponse<List<StableHorseDto>?>.Failure(HttpStatusCode.NotFound,
                    "Error: Stable does not have any horses connected to it.");
            
            return ApiResponse<List<StableHorseDto>?>.Success(HttpStatusCode.OK,
                stableHorseDtos,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<StableHorseDto>>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
<<<<<<< HEAD

    public async Task<ApiResponse<List<StableHorseOwnersDto>?>> GetHorsesWithOwnersByStableAsync(int stableId)
    {
        try
        {
            var stableHorses = await context.StableHorses
            .Where(sh => sh.StableIdFk == stableId)
            .Include(sh => sh.Horse)
                .ThenInclude(h => h.UserHorses)
                    .ThenInclude(uh => uh.User)
            .ToListAsync();

            if (stableHorses == null || !stableHorses.Any())
            {
                return ApiResponse<List<StableHorseOwnersDto>>.Failure(HttpStatusCode.NotFound,
                    $"No horses found for stable with ID {stableId}");
            }

            var stableHorseOwnersDtos = mapper.Map<List<StableHorseOwnersDto>>(stableHorses);

            return ApiResponse<List<StableHorseOwnersDto>>.Success(HttpStatusCode.OK,
                stableHorseOwnersDtos,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<StableHorseOwnersDto>>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> CreateStableHorseConnectionAsync(int stableId, int horseId)
=======
    
    public async Task<ApiResponse<int>> CreateStableHorseConnectionAsync(int stableId, int horseId)
>>>>>>> main
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
        
            return ApiResponse<int>.Success(HttpStatusCode.Created,
                stableHorse.Id,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<int>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> RemoveHorseFromStableAsync(int stableHorseId)
    {
        try
        {
            var stableHorse = await context.StableHorses
                .Where(sh => sh.Id == stableHorseId)
                .FirstOrDefaultAsync();
            
            if (stableHorse == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotAcceptable,
                    "Error: Relation entity not found.");

            context.StableHorses.Remove(stableHorse);
            await context.SaveChangesAsync();
            
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