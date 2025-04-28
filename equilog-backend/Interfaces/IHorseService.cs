using equilog_backend.Common;
using equilog_backend.DTOs.HorseDTOs;

namespace equilog_backend.Interfaces;

public interface IHorseService
{
    Task<ApiResponse<List<HorseDto>?>> GetHorsesAsync();

    Task<ApiResponse<HorseDto?>> GetHorseAsync(int id);

    Task<ApiResponse<HorseDto?>> CreateHorseAsync(HorseCreateDto horseCreateDto);

    Task<ApiResponse<Unit>> UpdateHorseAsync(HorseUpdateDto horseUpdateDto);

    Task<ApiResponse<Unit>> DeleteHorseAsync(int id);
}