using equilog_backend.Common;
using equilog_backend.DTOs.StablePostDTOs;

namespace equilog_backend.Interfaces;

public interface IStablePostService
{
    Task<ApiResponse<List<StablePostDto>?>> GetStablePostsByStableIdAsync(int stableId);

    Task<ApiResponse<StablePostDto?>> GetStablePostByStablePostIdAsync(int stablePostId);

    Task<ApiResponse<StablePostDto?>> CreateStablePostAsync(StablePostCreateDto stablePostCreateDto);

    Task<ApiResponse<Unit>> UpdateStablePostAsync(StablePostUpdateDto stablePostUpdateDto);

    Task<ApiResponse<Unit>> ChangeStablePostIsPinnedFlagAsync(int stablePostId);

    Task<ApiResponse<Unit>> DeleteStablePostAsync(int stablePostId);
}