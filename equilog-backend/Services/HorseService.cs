using equilog_backend.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Net;
using equilog_backend.Common;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using equilog_backend.DTOs.UserHorseDTOs;
using AutoMapper.QueryableExtensions;

namespace equilog_backend.Services;

public class HorseService(EquilogDbContext context, IMapper mapper) : IHorseService
{
    public async Task<ApiResponse<List<HorseDto>?>> GetHorsesAsync()
    {
        try
        {
            var horseDtos = mapper.Map<List<HorseDto>>(await context.Horses.ToListAsync());
    
            return ApiResponse<List<HorseDto>>.Success(HttpStatusCode.OK,
                horseDtos,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<HorseDto>>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<HorseDto?>> GetHorseAsync(int horseId)
    {
        try
        {  
            var horse = await context.Horses
                .Where(h => h.Id == horseId)
                .FirstOrDefaultAsync();

            if (horse == null)
                return ApiResponse<HorseDto>.Failure(HttpStatusCode.NotFound,
                "Error: Horse not found");

            return ApiResponse<HorseDto>.Success(HttpStatusCode.OK,
                mapper.Map<HorseDto>(horse),
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<HorseDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<HorseProfileDto?>> GetHorseProfileAsync(int horseId)
    {
        try
        {
            var horse = mapper.Map<HorseDto>(await context.Horses
                .Where(h => h.Id == horseId)
                .FirstOrDefaultAsync());

            if (horse == null)
            {
                return ApiResponse<HorseProfileDto>.Failure(HttpStatusCode.NotFound,
                "Error: Horse not found");
            }

            var userWithHorseRoleDtos = await context.UserHorses
                .Where(uh => uh.HorseIdFk == horseId)
                .ProjectTo<UserWithUserHorseRoleDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            var horseProfileDto = new HorseProfileDto 
            {
                Horse = horse,
                UserHorseRoles = userWithHorseRoleDtos 
            };

            return ApiResponse<HorseProfileDto>.Success(HttpStatusCode.OK,
                horseProfileDto,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<HorseProfileDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    public async Task<ApiResponse<int>> CreateHorseAsync(HorseCreateDto horseCreateDto)
    {
        try
        {
            var horse = mapper.Map<Horse>(horseCreateDto);

            context.Horses.Add(horse);
            await context.SaveChangesAsync();

            return ApiResponse<int>.Success(HttpStatusCode.Created,
                horse.Id,
                "Horse created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<int>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
           
    }

    public async Task<ApiResponse<Unit>> UpdateHorseAsync(HorseUpdateDto horseUpdateDto)
    {
        try
        {
            var horse = await context.Horses
                .Where(h => h.Id == horseUpdateDto.Id)
                .FirstOrDefaultAsync();
                
            if (horse == null) 
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound ,
                "Error: Horse not found");

            mapper.Map(horseUpdateDto, horse);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                "Horse information updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> DeleteHorseAsync(int horseId)
    {
        try
        {
            var horse = await context.Horses
                .Where(h => h.Id == horseId)
                .FirstOrDefaultAsync();

            if (horse == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                "Error: Horse not found");

            context.Horses.Remove(horse);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(HttpStatusCode.NoContent,
                Unit.Value,
                $"Horse with id '{horseId}' was deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}