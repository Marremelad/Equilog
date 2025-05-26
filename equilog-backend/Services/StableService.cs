using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

// Service that manages stable entities including retrieval, creation, updates, and search functionality.
// Handles horse stable information with member and horse counts, plus advanced search capabilities.
public class StableService(EquilogDbContext context, IMapper mapper) : IStableService
{
    // Retrieves detailed information about a specific stable including member and horse counts.
    public async Task<ApiResponse<StableDto?>> GetStableByStableIdAsync(int stableId)
    {
        try
        {
            // Fetch the stable with related data for calculating counts.
            var stable = await context.Stables
                .Include(s => s.UserStables)
                .Include(s => s.StableHorses)
                .Where(s => s.Id == stableId)
                .FirstOrDefaultAsync();
            
            // Returns an error if the stable doesn't exist.
            if (stable == null)
                return ApiResponse<StableDto>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Stable not found.");
            
            return ApiResponse<StableDto>.Success(
                HttpStatusCode.OK,
                mapper.Map<StableDto>(stable),
                "Stable fetched successfully.");
        }
        catch (Exception ex)
        {
           return ApiResponse<StableDto?>.Failure(
               HttpStatusCode.InternalServerError,
               ex.Message);
        }
    }
    
    // Searches for stables based on name, county, or address with pagination and intelligent ranking.
    public async Task<ApiResponse<List<StableSearchDto>?>> SearchStablesAsync(
        StableSearchParametersDto stableSearchParametersDto)
    {
        try
        {
            var searchTerm = stableSearchParametersDto.SearchTerm;
            var page = stableSearchParametersDto.Page;
            var pageSize = stableSearchParametersDto.PageSize;
            
            // Sanitize and validate pagination parameters.
            page = Math.Max(0, page);
            pageSize = Math.Clamp(pageSize, 1, 50);
            
            // Truncate search term to prevent excessively long queries.
            if (searchTerm.Length > 50)
            {
                searchTerm = searchTerm.Substring(0, 50);
            }
            
            // Start with a base query for all stables.
            var query = context.Stables.AsNoTracking();

            // Apply search filtering and intelligent ranking if search term provided.
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim()
                    .ToLower();

                // Create search patterns for exact prefix matches and contains matches.
                var starts = $"{term}%";
                var contains = $"%{term}%";

                // Filter stables that match the search term in name, county, or address.
                query = query
                    .Where(s =>
                        EF.Functions.Like(s.Name, starts) ||
                        EF.Functions.Like(s.Name, contains) ||
                        EF.Functions.Like(s.County, contains) ||
                        EF.Functions.Like(s.Address, contains)
                    );

                // Rank results with name prefix matches first, then other matches.
                query = query
                    .OrderBy(s =>
                         EF.Functions.Like(s.Name, starts) ? 0 :
                         EF.Functions.Like(s.Name, contains) ? 1 :
                         EF.Functions.Like(s.County, contains) ? 2 :
                         EF.Functions.Like(s.Address, contains) ? 3 : 4)
                    .ThenBy(s => s.Name);
            }
            else
            {
                // No search term provided - order alphabetically by name.
                query = query.OrderBy(s => s.Name);
            }

            // Apply pagination to the filtered and ordered results.
            var pagedResults = await query
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false);

            return ApiResponse<List<StableSearchDto>>.Success(
                HttpStatusCode.OK,
                mapper.Map<List<StableSearchDto>>(pagedResults),
                "Stables fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<StableSearchDto>>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Creates a new stable in the system and returns its generated ID.
    public async Task<ApiResponse<int>> CreateStableAsync(StableCreateDto stableCreateDto)
   {
      try
      {
          // Map the creation DTO to a stable entity.
            var stable = mapper.Map<Stable>(stableCreateDto);

            // Add the stable to the database and save to generate ID.
            context.Stables.Add(stable);
            await context.SaveChangesAsync();

            // Return the generated stable ID for use in relationship creation.
            return ApiResponse<int>.Success(
                HttpStatusCode.Created, 
                stable.Id,
                "Stable created successfully.");
      }
      catch (Exception ex)
      {
         return ApiResponse<int>.Failure(
             HttpStatusCode.InternalServerError, 
             ex.Message);
      }
   }

    // Updates an existing stable with new information.
   public async Task<ApiResponse<Unit>> UpdateStableAsync(StableUpdateDto stableUpdateDto)
   {
      try
      {
          // Find the existing stable to update.
         var stable = await context.Stables
            .Where(s => s.Id == stableUpdateDto.Id)
            .FirstOrDefaultAsync();

         // Returns an error if the stable doesn't exist.
         if (stable == null)
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.NotFound,
                "Error: Stable not found.");

         // Apply updates from DTO to an existing entity and save changes.
         mapper.Map(stableUpdateDto, stable);
         await context.SaveChangesAsync();

         return ApiResponse<Unit>.Success(
             HttpStatusCode.OK,
             Unit.Value,
             "Stable information updated successfully.");
      }
      catch (Exception ex)
      {
         return ApiResponse<Unit>.Failure(
             HttpStatusCode.InternalServerError, 
             ex.Message);
      }
   }

   // Removes a stable from the system along with all its relationships.
   public async Task<ApiResponse<Unit>> DeleteStableAsync(int stableId)
   {
      try
      {
          // Find the stable to delete.
         var stable = await context.Stables
            .Where(s => s.Id == stableId)
            .FirstOrDefaultAsync();

         // Returns an error if the stable doesn't exist.
         if (stable == null)
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.NotFound,
                "Error: stable not found");

         // Remove the stable (cascade delete will handle relationship cleanup).
         context.Stables.Remove(stable);
         await context.SaveChangesAsync();

         return ApiResponse<Unit>.Success(
             HttpStatusCode.OK,
             Unit.Value,
             $"Stable with id '{stableId}' was deleted successfully");
      }
      catch (Exception ex)
      {
         return ApiResponse<Unit>.Failure(
             HttpStatusCode.InternalServerError,
             ex.Message);
      }
   }
}