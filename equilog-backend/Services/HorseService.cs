using equilog_backend.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Net;
using equilog_backend.Common;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.Models;

namespace equilog_backend.Services;

public class HorseService(EquilogDbContext context, IMapper mapper)
{
    public async Task<ApiResponse<List<HorseDto>?>> GetHorses()
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

    public async Task<ApiResponse<HorseDto?>> GetHorse(int id)
    {
        try
        {  
            var horse = await context.Horses
                .Where(h => h.Id == id)
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

    public async Task<ApiResponse<HorseDto?>> CreateHorse(HorseCreateDto newHorse)
    {
        try
        {
            var horse = mapper.Map<Horse>(newHorse);

            context.Horses.Add(horse);
            await context.SaveChangesAsync();

            return ApiResponse<HorseDto>.Success(HttpStatusCode.Created,
                mapper.Map<HorseDto>(horse),
                "Horse created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<HorseDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
           
    }

    public async Task<ApiResponse<HorseDto?>> UpdateHorse(HorseUpdateDto updatedHorse)
    {
        try
        {
            var horse = await context.Horses
                .Where(h => h.Id == updatedHorse.Id)
                .FirstOrDefaultAsync();
                
            if (horse == null) 
                return ApiResponse<HorseDto>.Failure(HttpStatusCode.NotFound ,
                "Error: Horse not found");

            mapper.Map(updatedHorse, horse);
            await context.SaveChangesAsync();

            return ApiResponse<HorseDto>.Success(HttpStatusCode.OK,
                mapper.Map<HorseDto>(horse),
                "Horse information updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<HorseDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<HorseDto?>> DeleteHorse(int id)
    {
        try
        {
            var horse = await context.Horses
                .Where(h => h.Id == id)
                .FirstOrDefaultAsync();

            if (horse == null)
                return ApiResponse<HorseDto>.Failure(HttpStatusCode.NotFound,
                "Error: Horse not found");

            context.Horses.Remove(horse);
            await context.SaveChangesAsync();

            return ApiResponse<HorseDto>.Success(HttpStatusCode.NoContent,
                null,
                $"Horse with id '{id}' was deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<HorseDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}