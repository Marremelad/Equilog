using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Common;

namespace equilog_backend.Interfaces
{
    public interface IWallPostService
    {
        public Task<ApiResponse<WallPostDto?>> GetWallPost(int stableId);

        public Task<ApiResponse<WallPostDto?>> ReplaceWallPost(WallPostDto wallPostDto);

        public Task<ApiResponse<WallPostDto?>> EditWallPost(WallPostDto wallPostDto);

        public Task<ApiResponse<WallPostDto?>> ClearWallPost(WallPostDto wallPostDto);
    }
}
