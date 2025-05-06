using equilog_backend.Common;
using equilog_backend.DTOs.StableDTOs;

namespace equilog_backend.Interfaces;

public interface IStableComposition
{
    Task<ApiResponse<Unit>> CreateStableComposition(StableCreateDto stableCreateDto);
}