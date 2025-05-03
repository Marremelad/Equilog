using equilog_backend.Common;
using equilog_backend.DTOs.StableDTOs;

namespace equilog_backend.Interfaces;

public interface IStableService
{
    Task<ApiResponse<List<StableDto>?>> GetStablesAsync();

    Task<ApiResponse<StableDto?>> GetStableAsync(int stableId);

    Task<ApiResponse<List<StableSearchDto>?>> SearchStablesAsync(string searchTerm, int page = 0, int pageSize = 10);

    Task<ApiResponse<StableDto?>> CreateStableAsync(StableCreateDto stableCreateDto);

    Task<ApiResponse<Unit>> UpdateStableAsync(StableUpdateDto stableUpdateDto);

    Task<ApiResponse<Unit>> DeleteStableAsync(int stableId);
}