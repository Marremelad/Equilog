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
        public async Task<ApiResponse<WallPostDto?>> GetWallPostAsync(int stableId)
        {
            try
            {
                var wallPost = await context.WallPosts
                    .Where(wp => wp.StableIdFk == stableId)
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
        
        public async Task<ApiResponse<WallPostDto?>> CreateWallPostAsync(int stableId)
        {
            try
            {
                var stable = await context.Stables
                    .Include(s => s.WallPost)
                    .Where(s => s.Id == stableId)
                    .FirstOrDefaultAsync();

                if (stable == null) // blocks if stable dont exist
                {
                    return ApiResponse<WallPostDto>.Failure(HttpStatusCode.NotFound,
                        $"Error: Cannot create wall post for non-existent stable with ID {stableId}");
                }
                if (stable.WallPost != null) // blocks if stable has a wall post already
                {
                    return ApiResponse<WallPostDto?>.Failure(HttpStatusCode.Conflict,
                        $"Error: Wall post for stable with ID: {stableId} already exists");
                }

                var wallPost = new WallPost{ StableIdFk = stableId };
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

        public async Task<ApiResponse<Unit>> ReplaceWallPostAsync(WallPostReplaceDto wallPostReplaceDto)
        {
            try
            {
                var wallPost = await context.WallPosts
                    .Where(wp => wp.StableIdFk == wallPostReplaceDto.StableIdFk)
                    .FirstOrDefaultAsync();

                if (wallPost == null)
                {
                    return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Error: Wall post not found");
                }
                    
                mapper.Map(wallPostReplaceDto, wallPost);
                wallPost.PostDate = DateTime.UtcNow;
                wallPost.LastEdited = null;
                await context.SaveChangesAsync();

                return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                    Unit.Value,
                    "New wall post created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }

        public async Task<ApiResponse<Unit>> EditWallPostAsync(WallPostEditDto wallPostEditDto)
        {
            try
            {
                var wallPost = await context.WallPosts
                    .Where(wp => wp.StableIdFk == wallPostEditDto.StableIdFk)
                    .FirstOrDefaultAsync();

                if (wallPost == null)
                    return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Error: Wall post not found");

                mapper.Map(wallPostEditDto, wallPost);
                wallPost.LastEdited = DateTime.UtcNow;
                await context.SaveChangesAsync();

                return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                    Unit.Value,
                    "Wall post edited successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }

        public async Task<ApiResponse<Unit>> ClearWallPostAsync(int stableId)
        {
            try
            {
                var wallPost = await context.WallPosts
                    .Where(wp => wp.StableIdFk == stableId)
                    .FirstOrDefaultAsync();

                if (wallPost == null)
                {
                    return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                        "Error: Wall post not found for this stable");
                }

                var clearPost = new WallPostClearDto();
                mapper.Map(clearPost, wallPost);
                await context.SaveChangesAsync();

                return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                    Unit.Value,
                    "Wall post cleared successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }
    }
}
