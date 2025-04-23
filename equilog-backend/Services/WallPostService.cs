using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.WallPostDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace equilog_backend.Services
{
    public class WallPostService(EquilogDbContext context, IMapper mapper) : IWallPostService
    {
        public async Task<ApiResponse<WallPostDto?>> GetWallPostAsync(int id)
        {
            try
            {
                var wallPost = await context.WallPosts
                    .Where(wp => wp.StableIdFk == id)
                    .FirstOrDefaultAsync();

                if (wallPost == null)
                {
                    return ApiResponse<WallPostDto>.Failure(HttpStatusCode.NotFound,
                    "Error: Wall post not found");
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

        public async Task<ApiResponse<WallPostDto?>> NewWallPostAsync(WallPostReplaceDto wallPostReplaceDto)
        {
            try
            {
                var wallPost = await context.WallPosts
                    .Where(wp => wp.StableIdFk == wallPostReplaceDto.StableIdFk)
                    .FirstOrDefaultAsync();

                if (wallPost == null)
                {
                    return ApiResponse<WallPostDto>.Failure(HttpStatusCode.NotFound,
                    "Error: Wall post not found");
                }
                    
                mapper.Map(wallPostReplaceDto, wallPost);
                wallPost.PostDate = DateTime.UtcNow;
                wallPost.LastEdited = null;
                await context.SaveChangesAsync();

                return ApiResponse<WallPostDto>.Success(HttpStatusCode.OK,
                    mapper.Map<WallPostDto>(wallPost),
                    "New wall post created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<WallPostDto>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }

        public async Task<ApiResponse<WallPostDto?>> EditWallPostAsync(WallPostEditDto wallPostEditDto)
        {
            try
            {
                var wallPost = await context.WallPosts
                    .Where(wp => wp.StableIdFk == wallPostEditDto.StableIdFk)
                    .FirstOrDefaultAsync();

                if (wallPost == null)
                    return ApiResponse<WallPostDto>.Failure(HttpStatusCode.NotFound,
                    "Error: Wall post not found");

                mapper.Map(wallPostEditDto, wallPost);
                wallPost.LastEdited = DateTime.UtcNow;
                await context.SaveChangesAsync();

                return ApiResponse<WallPostDto>.Success(HttpStatusCode.OK,
                    mapper.Map<WallPostDto>(wallPost),
                    "Wall post edited successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<WallPostDto>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }

        public async Task<ApiResponse<WallPostDto?>> ClearWallPostAsync(int id)
        {
            try
            {
                var wallPost = await context.WallPosts
                    .Where(wp => wp.StableIdFk == id)
                    .FirstOrDefaultAsync();

                if (wallPost == null)
                {
                    return ApiResponse<WallPostDto>.Failure(HttpStatusCode.NotFound,
                        "Error: Wall post not found for this stable");
                }

                var clearPost = new WallPostClearDto();
                mapper.Map(clearPost, wallPost);
                await context.SaveChangesAsync();

                return ApiResponse<WallPostDto>.Success(HttpStatusCode.OK,
                    mapper.Map<WallPostDto>(wallPost),
                    "Wall post cleared successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<WallPostDto>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }

        public async Task<ApiResponse<WallPostDto?>> CreateWallPostAsync(int id)
        {
            try
            {
                var stable = await context.Stables
                    .Include(s => s.WallPost)
                    .Where(s => s.Id == id)
                    .FirstOrDefaultAsync();

                if (stable == null) // blocks if stable dont exist
                {
                    return ApiResponse<WallPostDto>.Failure(HttpStatusCode.NotFound,
                    $"Error: Cannot create wall post for non-existent stable with ID {id}");
                }
                if (stable.WallPost != null) // blocks if stable has a wall post already
                {
                    return ApiResponse<WallPostDto?>.Failure(HttpStatusCode.Conflict,
                    $"Error: Wall post for stable with ID: {id} already exists");
                }

                var wallPost = new WallPost{ StableIdFk = id };
                context.WallPosts.Add(wallPost);
                await context.SaveChangesAsync();

                return ApiResponse<WallPostDto?>.Success(HttpStatusCode.Created,
                    mapper.Map<WallPostDto>(wallPost),
                    "Wall post created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<WallPostDto>.Failure(HttpStatusCode.InternalServerError,
                    $"Error creating wall post: {ex.Message}");
            }
        }
    }
}
