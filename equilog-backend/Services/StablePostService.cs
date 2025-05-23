using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StablePostDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class StablePostService(EquilogDbContext context, IMapper mapper) : IStablePostService
{
    public async Task<ApiResponse<List<StablePostDto>?>> GetStablePostsAsync(int stableId)
    {
        try
        {
            var stablePosts = await context.StablePosts
                .Where(sp => sp.StableIdFk == stableId)
                .Include(sp => sp.User)
                .ToListAsync();
            
            var stablePostDtos = mapper.Map<List<StablePostDto>>(stablePosts);
            
             var message = stablePostDtos.Count == 0
                ? "Operation successful but stable has no stable-posts."
                : "Stable-posts fetched successfully.";

            return ApiResponse<List<StablePostDto>>.Success(HttpStatusCode.OK,
                stablePostDtos,
                message);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<StablePostDto>>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<StablePostDto?>> GetStablePostAsync(int stablePostId)
    {
        try
        {  
            var stablePost = await context.StablePosts
                .Include(sp => sp.User)
                .Where(sp => sp.Id == stablePostId)
                .FirstOrDefaultAsync();

            if (stablePost == null)
                return ApiResponse<StablePostDto>.Failure(HttpStatusCode.NotFound,
                "Error: Stable-post not found.");

            return ApiResponse<StablePostDto>.Success(HttpStatusCode.OK,
                mapper.Map<StablePostDto>(stablePost),
                "Stable-post fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<StablePostDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<StablePostDto?>> CreateStablePostAsync(StablePostCreateDto stablePostCreateDto)
    {
        try
        {
            var stablePost = mapper.Map<StablePost>(stablePostCreateDto);

            context.StablePosts.Add(stablePost);
            await context.SaveChangesAsync();

            return ApiResponse<StablePostDto>.Success(HttpStatusCode.Created,
                mapper.Map<StablePostDto>(stablePost),
                "Stable-post created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<StablePostDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
           
    }

    public async Task<ApiResponse<Unit>> UpdateStablePostAsync(StablePostUpdateDto stablePostUpdateDto)
    {
        try
        {
            var stablePost = await context.StablePosts
                .Where(sp => sp.Id == stablePostUpdateDto.Id)
                .FirstOrDefaultAsync();
                
            if ( stablePost == null) 
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound ,
                "Error: Stable-post not found.");

            mapper.Map(stablePostUpdateDto, stablePost);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                "Stable-post information updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> ChangeStablePostIsPinnedFlagAsync(int id)
    {
        try
        {
            var stablePost = await context.StablePosts
                .Where(sp => sp.Id == id)
                .FirstOrDefaultAsync();
            
            if (stablePost == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Error: Stable-post not found.");

            stablePost.IsPinned = !stablePost.IsPinned;
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                "IsPinned flag for stable-post was changed successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> DeleteStablePostAsync(int stablePostId)
    {
        try
        {
            var stablePost = await context.StablePosts
                .Where(sp => sp.Id == stablePostId)
                .FirstOrDefaultAsync();

            if (stablePost == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                "Error: Stable-post not found.");

            context.StablePosts.Remove(stablePost);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                $"Stable-post with id '{stablePostId}' was deleted successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}