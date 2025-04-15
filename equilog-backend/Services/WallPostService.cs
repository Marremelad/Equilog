using equilog_backend.Common;
using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Services
{
    public class WallPostService : IWallPostService
    {
        public Task<ApiResponse<WallPostDto?>> GetWallPost(int StableId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<WallPostDto?>> UpdateEditWallPost(WallPostUpdateDto editWallPost)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<WallPostDto?>> UpdateReplaceWallPost(WallPostUpdateDto updatedWallPost)
        {
            throw new NotImplementedException();
        }
    }
}
