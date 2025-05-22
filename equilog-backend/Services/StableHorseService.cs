using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableHorseDTOs;
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

            var message = stableHorseDtos.Count == 0
                ? "Operation was successful but the stable has no horses."
                : "Horses fetched successfully";
            
            return ApiResponse<List<StableHorseDto>?>.Success(HttpStatusCode.OK,
                stableHorseDtos,
                message);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<StableHorseDto>>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<List<StableHorseOwnersDto>?>> GetHorsesWithOwnersByStableIdAsync(int stableId)
    {
        try
        {
            var stableHorseOwnersDtos = mapper.Map<List<StableHorseOwnersDto>>(
                await context.StableHorses
                .Where(sh => sh.StableIdFk == stableId)
                .Include(sh => sh.Horse)
                .ThenInclude(h => h!.UserHorses)!
                .ThenInclude(uh => uh.User)
                .ToListAsync());

            var message = stableHorseOwnersDtos.Count == 0
                ? "Operation was successful but the stable has no horses."
                : "Horses fetched successfully.";
            
            return ApiResponse<List<StableHorseOwnersDto>>.Success(HttpStatusCode.OK,
                stableHorseOwnersDtos,
                message);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<StableHorseOwnersDto>>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
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
                "Connection between stable and horse was created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
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
                    "Error: Connection between horse and stable was not found.");

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