using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Common;

namespace equilog_backend.Interfaces
{
    public interface IWallPostService
    {
        public Task<ApiResponse<WallPostDto?>> GetWallPost(int stableId);

        public Task<ApiResponse<WallPostDto?>> ReplaceWallPost(UpdateWallPostDto updateWallPostDto);

        public Task<ApiResponse<WallPostDto?>> EditWallPost(UpdateWallPostDto updateWallPostDto);

        public Task<ApiResponse<WallPostDto?>> ClearWallPost(ClearWallPostDto clearWallPostDto);
    }
}
