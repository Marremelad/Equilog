using equilog_backend.Common;
using equilog_backend.DTOs.HorseCompositionDTOs;

namespace equilog_backend.Interfaces;

public interface IHorseComposition
{
    Task<ApiResponse<Unit>> CreateHorseCompositionAsync(HorseCompositionCreateDto horseCompositionCreateDto);
}