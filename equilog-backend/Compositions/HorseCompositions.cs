using equilog_backend.Common;
using equilog_backend.DTOs.HorseCompositionDTOs;
using equilog_backend.Interfaces;
using System.Net;

namespace equilog_backend.Compositions;

// Composition service that orchestrates the creation of a horse with all required relationships.
// Handles the complex operation of creating a horse and linking it to both stable and user.
public class HorseCompositions(
    IHorseService horseService,
    IStableHorseService stableHorseService,
    IUserHorseService userHorseService) : IHorseComposition
{
    // Creates a complete horse composition including the horse and all its relationships.
    public async Task<ApiResponse<Unit>> CreateHorseCompositionAsync(
        HorseCompositionCreateDto horseCompositionCreateDto)
    {
        try
        {
            // Step 1: Create the core horse entity.
            var horseResponse = await horseService.CreateHorseAsync(horseCompositionCreateDto.Horse);

            if (!horseResponse.IsSuccess)
                return ApiResponse<Unit>.Failure(
                    horseResponse.StatusCode,
                    $"Failed to create horse: {horseResponse.Message}");

            // Extract IDs for creating the relationships.
            var horseId = horseResponse.Value;
            var stableId = horseCompositionCreateDto.StableId;
            var userId = horseCompositionCreateDto.UserId;

            // Step 2: Create the stable-horse relationship (assign horse to stable).
            var stableHorseResponse = await stableHorseService.CreateStableHorseConnectionAsync(stableId, horseId);

            // If the stable-horse relationship fails, rollback by deleting the horse.
            if (!stableHorseResponse.IsSuccess)
            {
                await horseService.DeleteHorseAsync(horseId);
                stableHorseResponse.Message =
                    $"Failed to create connection between stable and horse: {stableHorseResponse.Message}. Horse creation was rolled back.";
                return stableHorseResponse;
            }
        
            // Step 3: Create the user-horse relationship (assign ownership to a user).
            var userHorseResponse = await userHorseService.CreateUserHorseConnectionAsync(userId, horseId);

            // If the user-horse relationship fails, rollback by deleting the horse.
            // Note: Stable-horse relationship will be automatically cleaned up via cascade delete.
            if (!userHorseResponse.IsSuccess)
            {
                await horseService.DeleteHorseAsync(horseId);
                userHorseResponse.Message =
                    $"Failed to create connection between user and horse: {userHorseResponse.Message}.Horse creation was rolled back.";
            }

            // All operations successful - return success response.
            return ApiResponse<Unit>.Success(
                HttpStatusCode.Created,
                Unit.Value,
                "Horse created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}