using equilog_backend.Common;
using equilog_backend.DTOs.StableDTOs;

namespace equilog_backend.Interfaces;

public interface IStableService
{
    public Task<ApiResponse<List<StableDto>?>> GetStablesAsync();

    public Task<ApiResponse<StableDto?>> GetStableAsync(int id);

    public Task<ApiResponse<StableDto?>> CreateStableAsync(StableCreateDto stableCreateDto);

    public Task<ApiResponse<StableDto?>> UpdateStableAsync(StableUpdateDto stableUpdateDto);

    public Task<ApiResponse<StableDto?>> DeleteStableAsync(int id);
}