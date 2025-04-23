using equilog_backend.Common;
using equilog_backend.DTOs.StableDTOs;

namespace equilog_backend.Interfaces;

public interface IStableService
{
    public Task<ApiResponse<List<StableDto>?>> GetStables();

    public Task<ApiResponse<StableDto?>> GetStable(int id);

    public Task<ApiResponse<StableDto?>> CreateStable(StableCreateDto stableCreateDto);

    public Task<ApiResponse<StableDto?>> UpdateStable(StableUpdateDto updatedStable);

    public Task<ApiResponse<StableDto?>> DeleteStable(int id);
}