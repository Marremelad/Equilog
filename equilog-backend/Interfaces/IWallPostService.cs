using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Common;

namespace equilog_backend.Interfaces
{
    public interface IWallPostService
    {
        public Task<ApiResponse<WallPostDto?>> GetWallPost(int StableId);

        public Task<ApiResponse<WallPostDto?>> UpdateReplaceWallPost(WallPostUpdateDto updatedWallPost);
        public Task<ApiResponse<WallPostDto?>> UpdateEditWallPost(WallPostUpdateDto editWallPost);
    }
}
