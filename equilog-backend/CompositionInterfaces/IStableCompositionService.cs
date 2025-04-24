using equilog_backend.Common;
using equilog_backend.CompositionDTOs;
using equilog_backend.DTOs.StableDTOs;

namespace equilog_backend.CompositionInterfaces;

public interface IStableCompositionService
{
    Task<ApiResponse<StableWithWallPostDto?>> CreateStableWithWallPostAsync(StableCreateDto stableCreateDto);
}