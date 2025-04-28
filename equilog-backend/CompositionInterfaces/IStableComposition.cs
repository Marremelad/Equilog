using equilog_backend.Common;
using equilog_backend.CompositionDTOs;
using equilog_backend.DTOs.StableDTOs;

namespace equilog_backend.CompositionInterfaces;

public interface IStableComposition
{
    Task<ApiResponse<Unit>> CreateStableWithWallPostAsync(StableCreateDto stableCreateDto);
}