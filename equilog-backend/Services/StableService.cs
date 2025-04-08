using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.Interfaces;
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

   public async Task<ApiResponse<StableDto?>> GetStable()
   {
      
   }

   public async Task<ApiResponse<StableDto?>> CreateStable()
   {
      throw new NotImplementedException();
   }

   public async Task<ApiResponse<StableDto?>> UpdateStable()
   {
      throw new NotImplementedException();
   }

   public async Task<ApiResponse<StableDto?>> DeleteStable()
   {
      throw new NotImplementedException();
   }
}