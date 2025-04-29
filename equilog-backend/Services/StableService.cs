using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class StableService(EquilogDbContext context, IMapper mapper) : IStableService
{
   public async Task<ApiResponse<List<StableDto>?>> GetStablesAsync()
   {
      try
      {
         var stableDtos = mapper.Map<List<StableDto>>(await context.Stables.ToListAsync());

         return ApiResponse<List<StableDto>>.Success(HttpStatusCode.OK,
            stableDtos,
            null);
      }
      catch (Exception ex)
      {
         return ApiResponse<List<StableDto>>.Failure(HttpStatusCode.InternalServerError,
            ex.Message);
      }
   }

   public async Task<ApiResponse<StableDto?>> GetStableAsync(int id)
   {
      try
      {
         var stable = await context.Stables
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();

         if (stable == null)
            return ApiResponse<StableDto>.Failure(HttpStatusCode.NotFound,
               "Error: Stable not found");

         return ApiResponse<StableDto>.Success(HttpStatusCode.OK,
            mapper.Map<StableDto>(stable),
            null);
      }
      catch (Exception ex)
      {
         return ApiResponse<StableDto>.Failure(HttpStatusCode.InternalServerError,
            ex.Message);
      }
   }
   public async Task<ApiResponse<List<StableDto>?>> SearchStablesAsync(string searchTerm, int page = 0, int pageSize = 10)
   {
        throw new NotImplementedException();
   }

   public async Task<ApiResponse<StableDto?>> CreateStableAsync(StableCreateDto stableCreateDto)
   {
      try
      {
            var stable = mapper.Map<Stable>(stableCreateDto);

            context.Stables.Add(stable);
            await context.SaveChangesAsync();

            return ApiResponse<StableDto>.Success(HttpStatusCode.Created,
            mapper.Map<StableDto>(stable),
            "Stable created successfully");
      }
      catch (Exception ex)
      {
         return ApiResponse<StableDto>.Failure(HttpStatusCode.InternalServerError,
            ex.Message);
      }
   }

   public async Task<ApiResponse<Unit>> UpdateStableAsync(StableUpdateDto stableUpdateDto)
   {
      try
      {
         var stable = await context.Stables
            .Where(s => s.Id == stableUpdateDto.Id)
            .FirstOrDefaultAsync();

         if (stable == null)
            return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
               "Error: Stable not found");

         mapper.Map(stableUpdateDto, stable);
         await context.SaveChangesAsync();

         return ApiResponse<Unit>.Success(HttpStatusCode.OK,
            Unit.Value,
            "Stable information updated successfully");
      }
      catch (Exception ex)
      {
         return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
            ex.Message);
      }
   }

   public async Task<ApiResponse<Unit>> DeleteStableAsync(int id)
   {
      try
      {
         var stable = await context.Stables
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();

         if (stable == null)
            return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
               "Error: stable not found");

         context.Stables.Remove(stable);
         await context.SaveChangesAsync();

         return ApiResponse<Unit>.Success(HttpStatusCode.NoContent,
            Unit.Value, 
            $"Stable with id '{id}' was deleted successfully");
      }
      catch (Exception ex)
      {
         return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
            ex.Message);
      }
   }
}