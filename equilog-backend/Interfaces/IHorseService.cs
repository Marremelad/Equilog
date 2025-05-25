using equilog_backend.Common;
using equilog_backend.DTOs.HorseDTOs;

namespace equilog_backend.Interfaces;

public interface IHorseService
{
    Task<ApiResponse<HorseProfileDto?>> GetHorseProfileAsync(int horseId);

    Task<ApiResponse<int>> CreateHorseAsync(HorseCreateDto horseCreateDto);

    Task<ApiResponse<Unit>> UpdateHorseAsync(HorseUpdateDto horseUpdateDto);

    Task<ApiResponse<Unit>> DeleteHorseAsync(int horseId);
    
    // Used for testing.
    Task<ApiResponse<List<HorseDto>?>> GetHorsesAsync();
    
    Task<ApiResponse<HorseDto?>> GetHorseAsync(int horseId);
}