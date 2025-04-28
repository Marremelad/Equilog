using equilog_backend.Common;
using equilog_backend.DTOs.StablePostDTOs;

namespace equilog_backend.Interfaces;

public interface IStablePostService
{
    Task<ApiResponse<List<StablePostDto>?>> GetStablePostsAsync();

    Task<ApiResponse<StablePostDto?>> GetStablePostAsync(int id);

    Task<ApiResponse<StablePostDto?>> CreateStablePostAsync(StablePostCreateDto stablePostCreateDto);

    Task<ApiResponse<Unit>> UpdateStablePostAsync(StablePostUpdateDto stablePostUpdateDto);

    Task<ApiResponse<Unit>> DeleteStablePostAsync(int id);
}