using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Common;

namespace equilog_backend.Interfaces
{
    public interface IWallPostService
    {
        Task<ApiResponse<WallPostDto?>> GetWallPostAsync(int id);

        Task<ApiResponse<WallPostDto?>> CreateWallPostAsync(int id);
        
        Task<ApiResponse<Unit>> ReplaceWallPostAsync(WallPostReplaceDto wallPostReplaceDto);

        Task<ApiResponse<Unit>> EditWallPostAsync(WallPostEditDto wallPostEditDto);

        Task<ApiResponse<Unit>> ClearWallPostAsync(int id);
    }
}
