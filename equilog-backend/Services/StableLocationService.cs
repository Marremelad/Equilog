using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableLocationDtos;
using equilog_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace equilog_backend.Services
{
    public class StableLocationService(EquilogDbContext context, IMapper mapper) : IStableLocationService
    {
        public async Task<ApiResponse<StableLocationDto?>> GetStableLocationAsync(string postcode)
        {
            try
            {
                var stableLocation = await context.StableLocation
                    .Where(p => p.PostCode == postcode)
                    .FirstOrDefaultAsync();

                if (stableLocation == null)
                    return ApiResponse<StableLocationDto>.Failure(HttpStatusCode.NotFound,
                    "Error: StableLocation not found");

                return ApiResponse<StableLocationDto>.Success(HttpStatusCode.OK,
                    mapper.Map<StableLocationDto>(stableLocation),
                    null);
            }
            catch (Exception ex)
            {
                return ApiResponse<StableLocationDto>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }
    }
}
