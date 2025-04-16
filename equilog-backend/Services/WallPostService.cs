using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.UserDTOs;
using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
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
                    .Where(wp => wp.StableIdFk == stableId)
                    .FirstOrDefaultAsync();

                if (wallPost == null)
                {
                    return ApiResponse<WallPostDto>.Failure(HttpStatusCode.NotFound,
                    "Error: WallPost not found");
                }

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

        public async Task<ApiResponse<WallPostDto?>> ReplaceWallPost(UpdateWallPostDto updateWallPostDto)
        {
            try
            {
                var wallPost = await context.WallPosts
                    .Where(wp => wp.StableIdFk == updateWallPostDto.StableIdFk)
                    .FirstOrDefaultAsync();

                if (wallPost == null)
                {
                    return ApiResponse<WallPostDto>.Failure(HttpStatusCode.NotFound,
                    "Error: WallPost not found");
                }
                    
                mapper.Map(updateWallPostDto, wallPost);
                wallPost.PostDate = DateTime.UtcNow;
                wallPost.LastEdited = null;
                await context.SaveChangesAsync();

                return ApiResponse<WallPostDto>.Success(HttpStatusCode.OK,
                    mapper.Map<WallPostDto>(wallPost),
                    "Posted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<WallPostDto>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }

        public async Task<ApiResponse<WallPostDto?>> EditWallPost(UpdateWallPostDto updateWallPostDto)
        {
            try
            {
                var wallPost = await context.WallPosts
                    .Where(wp => wp.StableIdFk == updateWallPostDto.StableIdFk)
                    .FirstOrDefaultAsync();

                if (wallPost == null)
                    return ApiResponse<WallPostDto>.Failure(HttpStatusCode.NotFound,
                    "Error: WallPost not found");

                mapper.Map(updateWallPostDto, wallPost);
                wallPost.LastEdited = DateTime.UtcNow;
                await context.SaveChangesAsync();

                return ApiResponse<WallPostDto>.Success(HttpStatusCode.OK,
                    mapper.Map<WallPostDto>(wallPost),
                    "Edited successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<WallPostDto>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }

        public async Task<ApiResponse<WallPostDto?>> ClearWallPost(ClearWallPostDto clearWallPostDto)
        {
            try
            {
                var wallPost = await context.WallPosts
                    .Where(wp => wp.StableIdFk == clearWallPostDto.StableIdFk)
                    .FirstOrDefaultAsync();

                if (wallPost == null)
                {
                    return ApiResponse<WallPostDto>.Failure(HttpStatusCode.NotFound,
                        "Error: Wall post not found for this stable");
                }

                wallPost.Title = null;
                wallPost.Body = null;
                wallPost.PostDate = null;
                wallPost.LastEdited = null;
                await context.SaveChangesAsync();

                return ApiResponse<WallPostDto>.Success(HttpStatusCode.OK,
                    mapper.Map<WallPostDto>(wallPost),
                    "Wallpost cleared successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<WallPostDto>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }
    }
}
