using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableLocationDtos;
using equilog_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;

namespace equilog_backend.Services
{
	public class StableLocationService(EquilogDbContext context, IMapper mapper) : IStableLocationService
	{
		public async Task<ApiResponse<StableLocationDto?>> GetStableLocationAsync(string postcode)
		{
			try
			{
				string postcodeDigits = Regex.Replace(postcode, @"\D", "");

				if (postcodeDigits.Length != 5)
					return ApiResponse<StableLocationDto>.Failure(HttpStatusCode.NotFound,
					"Error: Post code must contain exactly 5 digits.");

				var stableLocation = await context.StableLocation
					.Where(p => p.PostCode == postcodeDigits)
					.FirstOrDefaultAsync();

				if (stableLocation == null)
					return ApiResponse<StableLocationDto>.Failure(HttpStatusCode.NotFound,
					"Error: Post code not found.");

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
