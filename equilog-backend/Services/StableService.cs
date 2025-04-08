using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class StableService(EquilogDbContext context, IMapper mapper) : IStableService
{
   public async Task<ApiResponse<List<StableDto>?>> GetStables()
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

   public async Task<ApiResponse<StableDto?>> GetStable(int id)
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

   public async Task<ApiResponse<StableDto?>> CreateStable(StableCreateDto newStable)
   {
      try
      {
         var stable = mapper.Map<Stable>(newStable);

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

   public async Task<ApiResponse<StableDto?>> UpdateStable(StableUpdateDto updatedStable)
   {
      try
      {
         var stable = await context.Stables
            .Where(s => s.Id == updatedStable.Id)
            .FirstOrDefaultAsync();

         if (stable == null)
            return ApiResponse<StableDto>.Failure(HttpStatusCode.NotFound,
               "Error: Stable not found");

         mapper.Map(updatedStable, stable);
         await context.SaveChangesAsync();

         return ApiResponse<StableDto>.Success(HttpStatusCode.OK,
            mapper.Map<StableDto>(stable),
            "Stable information updated successfully");
      }
      catch (Exception ex)
      {
         return ApiResponse<StableDto>.Failure(HttpStatusCode.InternalServerError,
            ex.Message);
      }
   }

   public async Task<ApiResponse<StableDto?>> DeleteStable(int id)
   {
      throw new NotImplementedException();
   }
}