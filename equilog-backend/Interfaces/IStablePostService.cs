using equilog_backend.Common;
using equilog_backend.DTOs.StablePostDTOs;

namespace equilog_backend.Interfaces;

public interface IStablePostService
{
    public Task<ApiResponse<List<StablePostDto>?>> GetStablePosts();

    public Task<ApiResponse<StablePostDto?>> GetStablePost(int id);

    public Task<ApiResponse<StablePostDto?>> CreateStablePost(StablePostCreateDto newStablePost);

    public Task<ApiResponse<StablePostDto?>> UpdateStablePost(StablePostUpdateDto updatedStablePost);

    public Task<ApiResponse<StablePostDto?>> DeleteStablePost(int id);
}