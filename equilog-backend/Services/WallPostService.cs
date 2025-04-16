using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace equilog_backend.Services
{
    public class WallPostService(EquilogDbContext context, IMapper mapper) : IWallPostService
    {
        public async Task<ApiResponse<WallPostDto?>> GetWallPost(int stableId)
        {
            try
            {
                var wallPost = await context.WallPosts
                    .Where(s => s.Id == stableId)
                    .FirstOrDefaultAsync();

                if (wallPost == null)
                    return ApiResponse<WallPostDto>.Failure(HttpStatusCode.NotFound,
                    "Error: WallPost not found");

                return ApiResponse<WallPostDto>.Success(HttpStatusCode.OK,
                    mapper.Map<WallPostDto>(wallPost),
                    null);
            }
            catch (Exception ex)
            {
                return ApiResponse<WallPostDto>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }

        public Task<ApiResponse<WallPostDto?>> ReplaceWallPost(WallPostDto wallPostDto)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<WallPostDto?>> EditWallPost(WallPostDto wallPostDto)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<WallPostDto?>> ClearWallPost(WallPostDto wallPostDto)
        {
            throw new NotImplementedException();
        }
    }
}
