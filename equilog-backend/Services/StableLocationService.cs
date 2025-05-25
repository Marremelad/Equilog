using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableLocationDtos;
using equilog_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;

namespace equilog_backend.Services;

// Service that provides location information for stables based on Swedish postal codes.
// Offers geographic and administrative data to help with stable registration and location services.
public class StableLocationService(EquilogDbContext context, IMapper mapper) : IStableLocationService
{
	// Retrieves detailed location information for a given Swedish postal code.
	public async Task<ApiResponse<StableLocationDto?>> GetStableLocationAsync(string postcode)
	{
		try
		{
			// Extract only numeric digits from the postal code input.
			var postcodeDigits = Regex.Replace(postcode, @"\D", "");

			// Validate that the postal code has exactly 5 digits (Swedish standard).
			if (postcodeDigits.Length != 5)
				return ApiResponse<StableLocationDto>.Failure(
					HttpStatusCode.NotFound,
					"Error: Post code must contain exactly 5 digits.");

			// Search for the location data using the cleaned postal code.
			var stableLocation = await context.StableLocation
				.Where(p => p.PostCode == postcodeDigits)
				.FirstOrDefaultAsync();

			// Returns an error if no location data exists for the postal code.
			if (stableLocation == null)
				return ApiResponse<StableLocationDto>.Failure(
					HttpStatusCode.NotFound,
					"Error: Post code not found.");

			return ApiResponse<StableLocationDto>.Success(
				HttpStatusCode.OK,
				mapper.Map<StableLocationDto>(stableLocation),
				null);
		}
		catch (Exception ex)
		{
			return ApiResponse<StableLocationDto>.Failure(
				HttpStatusCode.InternalServerError,
				ex.Message);
		}
	}
}