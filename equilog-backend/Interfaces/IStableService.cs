using equilog_backend.Common;
using equilog_backend.DTOs.StableDTOs;

namespace equilog_backend.Interfaces;

public interface IStableService
{
    Task<ApiResponse<StableDto?>> GetStableByStableIdAsync(int stableId);
    
    Task<ApiResponse<List<StableSearchDto>?>> SearchStablesAsync(StableSearchParametersDto stableSearchParametersDto);

    Task<ApiResponse<int>> CreateStableAsync(StableCreateDto stableCreateDto);

    Task<ApiResponse<Unit>> UpdateStableAsync(StableUpdateDto stableUpdateDto);

    Task<ApiResponse<Unit>> DeleteStableAsync(int stableId);
}