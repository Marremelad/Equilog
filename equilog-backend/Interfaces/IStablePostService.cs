using equilog_backend.Common;
using equilog_backend.DTOs.StablePostDTOs;

namespace equilog_backend.Interfaces;

public interface IStablePostService
{
    public Task<ApiResponse<List<StablePostDto>?>> GetStablePostsAsync();

    public Task<ApiResponse<StablePostDto?>> GetStablePostAsync(int id);

    public Task<ApiResponse<StablePostDto?>> CreateStablePostAsync(StablePostCreateDto stablePostCreateDto);

    public Task<ApiResponse<Unit>> UpdateStablePostAsync(StablePostUpdateDto stablePostUpdateDto);

    public Task<ApiResponse<Unit>> DeleteStablePostAsync(int id);
}