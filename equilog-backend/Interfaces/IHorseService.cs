using equilog_backend.Common;
using equilog_backend.DTOs.HorseDTOs;

namespace equilog_backend.Interfaces;

public interface IHorseService
{
    public Task<ApiResponse<List<HorseDto>?>> GetHorses();

    public Task<ApiResponse<HorseDto?>> GetHorse(int id);

    public Task<ApiResponse<HorseDto?>> CreateHorse(HorseCreateDto newHorse);

    public Task<ApiResponse<HorseDto?>> UpdateHorse(HorseUpdateDto updatedHorse);

    public Task<ApiResponse<HorseDto?>> DeleteHorse(int id);

}