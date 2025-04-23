using equilog_backend.Common;
using equilog_backend.DTOs.CompositionDTOs;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.DTOs.WallPostDTOs;

namespace equilog_backend.Interfaces;

public interface IStableCompositionService
{
    Task<ApiResponse<StableWithWallPostDto?>> CreateStableWithWallPostAsync(StableCreateDto stableCreateDto);
}