using equilog_backend.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Net;
using equilog_backend.Common;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using equilog_backend.DTOs.UserHorseDTOs;

namespace equilog_backend.Services;

// Service that manages horse entities and their relationships with users and stables.
// Handles CRUD operations for horses and provides detailed horse profile information.
public class HorseService(EquilogDbContext context, IMapper mapper) : IHorseService 
{ 
    // Retrieves a detailed horse profile including associated users and their roles.
    public async Task<ApiResponse<HorseProfileDto?>> GetHorseProfileAsync(int horseId)
    {
        try
        {
            // Fetch the horse with related user relationships and user details.
            var horse = await context.Horses
                .Include(h => h.UserHorses!)
                .ThenInclude(uh => uh.User)
                .Where(h => h.Id == horseId)
                .FirstOrDefaultAsync();

            // Returns an error if the horse doesn't exist.
            if (horse == null)
                return ApiResponse<HorseProfileDto>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Horse not found.");

            // Build a comprehensive horse profile with user role information.
            var horseProfileDto = new HorseProfileDto 
            {
                Horse = mapper.Map<HorseDto>(horse),
                UserHorseRoles = mapper.Map<List<UserWithUserHorseRoleDto>>(horse.UserHorses)
            };

            return ApiResponse<HorseProfileDto>.Success(
                HttpStatusCode.OK,
                horseProfileDto,
                "Horse profile fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<HorseProfileDto>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Creates a new horse in the system and returns its generated ID.
    public async Task<ApiResponse<int>> CreateHorseAsync(HorseCreateDto horseCreateDto)
    {
        try
        {
            // Map the creation DTO to a horse entity.
            var horse = mapper.Map<Horse>(horseCreateDto);

            // Add the horse to the database and save to generate ID.
            context.Horses.Add(horse);
            await context.SaveChangesAsync();

            // Return the generated horse ID for use in relationship creation.
            return ApiResponse<int>.Success(
                HttpStatusCode.Created,
                horse.Id,
                "Horse created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<int>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Updates an existing horse with new information.
    public async Task<ApiResponse<Unit>> UpdateHorseAsync(HorseUpdateDto horseUpdateDto)
    {
        try
        {
            // Find the existing horse to update.
            var horse = await context.Horses
                .Where(h => h.Id == horseUpdateDto.Id)
                .FirstOrDefaultAsync();
                
            // Returns an error if the horse doesn't exist.
            if (horse == null) 
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound ,
                "Error: Horse not found.");

            // Apply updates from DTO to the existing entity and save changes.
            mapper.Map(horseUpdateDto, horse);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "Horse information updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Removes a horse from the system along with all its relationships.
    public async Task<ApiResponse<Unit>> DeleteHorseAsync(int horseId)
    {
        try
        {
            // Find the horse to delete.
            var horse = await context.Horses
                .Where(h => h.Id == horseId)
                .FirstOrDefaultAsync();

            // Returns an error if the horse doesn't exist.
            if (horse == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                "Error: Horse not found");

            // Remove the horse (cascade delete will handle relationship cleanup).
            context.Horses.Remove(horse);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                $"Horse with id '{horseId}' was deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Retrieves all horses in the system (used for testing purposes).
    public async Task<ApiResponse<List<HorseDto>?>> GetHorsesAsync()
    {
        try
        {
            // Fetch all horses and map to DTOs.
            var horseDtos = mapper.Map<List<HorseDto>>(await context.Horses.ToListAsync());
    
            return ApiResponse<List<HorseDto>>.Success(
                HttpStatusCode.OK,
                horseDtos,
                "Horses fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<HorseDto>>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Retrieves a specific horse by its ID (used for testing purposes).
    public async Task<ApiResponse<HorseDto?>> GetHorseAsync(int horseId)
    {
        try
        {  
            // Find the horse by ID.
            var horse = await context.Horses
                .Where(h => h.Id == horseId)
                .FirstOrDefaultAsync();
    
            // Returns an error if the horse doesn't exist.
            if (horse == null)
                return ApiResponse<HorseDto>.Failure(
                    HttpStatusCode.NotFound,
                "Error: Horse not found.");
    
            return ApiResponse<HorseDto>.Success(
                HttpStatusCode.OK,
                mapper.Map<HorseDto>(horse),
                "Horse fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<HorseDto>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}