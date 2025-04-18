using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Common;

namespace equilog_backend.Interfaces
{
    public interface IWallPostService
    {
        public Task<ApiResponse<WallPostDto?>> GetWallPost(int stableId);

        public Task<ApiResponse<WallPostDto?>> ReplaceWallPost(WallPostReplaceDto wallPostReplaceDto);

        public Task<ApiResponse<WallPostDto?>> EditWallPost(WallPostEditDto wallPostEditDto);

        public Task<ApiResponse<WallPostDto?>> ClearWallPost(int stableId);

        public Task<ApiResponse<WallPostDto?>> CreateWallPost(int stableId);
    }
}
