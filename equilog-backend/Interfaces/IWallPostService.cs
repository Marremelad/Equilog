using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Common;

namespace equilog_backend.Interfaces
{
    public interface IWallPostService
    {
        public Task<ApiResponse<WallPostDto?>> GetWallPostAsync(int id);

        public Task<ApiResponse<WallPostDto?>> CreateWallPostAsync(int id);
        
        public Task<ApiResponse<Unit>> ReplaceWallPostAsync(WallPostReplaceDto wallPostReplaceDto);

        public Task<ApiResponse<Unit>> EditWallPostAsync(WallPostEditDto wallPostEditDto);

        public Task<ApiResponse<Unit>> ClearWallPostAsync(int id);
    }
}
