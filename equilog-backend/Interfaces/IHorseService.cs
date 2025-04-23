using equilog_backend.Common;
using equilog_backend.DTOs.HorseDTOs;

namespace equilog_backend.Interfaces;

public interface IHorseService
{
    public Task<ApiResponse<List<HorseDto>?>> GetHorsesAsync();

    public Task<ApiResponse<HorseDto?>> GetHorseAsync(int id);

    public Task<ApiResponse<HorseDto?>> CreateHorseAsync(HorseCreateDto horseCreateDto);

    public Task<ApiResponse<HorseDto?>> UpdateHorseAsync(HorseUpdateDto horseUpdateDto);

    public Task<ApiResponse<HorseDto?>> DeleteHorseAsync(int id);

}