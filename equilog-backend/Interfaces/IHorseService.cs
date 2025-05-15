using equilog_backend.Common;
using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.DTOs.UserHorseDTOs;

namespace equilog_backend.Interfaces;

public interface IHorseService
{
    Task<ApiResponse<List<HorseDto>?>> GetHorsesAsync();

    Task<ApiResponse<HorseDto?>> GetHorseAsync(int horseId);

    Task<ApiResponse<HorseProfileDto?>> GetHorseProfileAsync(int horseId);

    Task<ApiResponse<HorseDto?>> CreateHorseAsync(HorseCreateDto horseCreateDto);

    Task<ApiResponse<Unit>> UpdateHorseAsync(HorseUpdateDto horseUpdateDto);

    Task<ApiResponse<Unit>> DeleteHorseAsync(int horseId);
}