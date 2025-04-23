using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Common;

namespace equilog_backend.Interfaces
{
    public interface IWallPostService
    {
        public Task<ApiResponse<WallPostDto?>> GetWallPostAsync(int id);

        public Task<ApiResponse<WallPostDto?>> NewWallPostAsync(WallPostReplaceDto wallPostReplaceDto);

        public Task<ApiResponse<WallPostDto?>> EditWallPostAsync(WallPostEditDto wallPostEditDto);

        public Task<ApiResponse<WallPostDto?>> ClearWallPostAsync(int id);

        public Task<ApiResponse<WallPostDto?>> CreateWallPostAsync(int id);
    }
}
