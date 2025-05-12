using equilog_backend.Common;
using equilog_backend.DTOs.HorseCompositionDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Compositions;

public class HorseComposition : IHorseComposition
{
    public Task<ApiResponse<Unit>> CreateHorseCompositionAsync(HorseCompositionCreateDto horseCompositionCreateDto)
    {
        throw new NotImplementedException();
    }
}