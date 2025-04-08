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
    public async Task<ApiResponse<List<StablePostDto>?>> GetStablePosts()
    {
        try
        {
            var stablePostDtos = mapper.Map<List<StablePostDto>>(await context.StablePosts.ToListAsync());
    
            return ApiResponse<List<StablePostDto>>.Success(HttpStatusCode.OK,
                stablePostDtos,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<StablePostDto>>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<StablePostDto?>> GetStablePost(int id)
    {
        try
        {  
            var stablePost = await context.StablePosts
                .Where(sp => sp.Id == id)
                .FirstOrDefaultAsync();

            if (stablePost == null)
                return ApiResponse<StablePostDto>.Failure(HttpStatusCode.NotFound,
                "Error: Stable post not found");

            return ApiResponse<StablePostDto>.Success(HttpStatusCode.OK,
                mapper.Map<StablePostDto>(stablePost),
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<StablePostDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<StablePostDto?>> CreateStablePost(StablePostCreateDto newStablePost)
    {
        try
        {
            var stablePost = mapper.Map<StablePost>(newStablePost);

            context.StablePosts.Add(stablePost);
            await context.SaveChangesAsync();

            return ApiResponse<StablePostDto>.Success(HttpStatusCode.Created,
                mapper.Map<StablePostDto>(stablePost),
                "Stable post created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<StablePostDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
           
    }

    public async Task<ApiResponse<StablePostDto?>> UpdateStablePost(StablePostUpdateDto updatedStablePost)
    {
        try
        {
            var stablePost = await context.StablePosts
                .Where(sp => sp.Id == updatedStablePost.Id)
                .FirstOrDefaultAsync();
                
            if ( stablePost == null) 
                return ApiResponse<StablePostDto>.Failure(HttpStatusCode.NotFound ,
                "Error: Stable post not found");

            mapper.Map(updatedStablePost, stablePost);
            await context.SaveChangesAsync();

            return ApiResponse<StablePostDto>.Success(HttpStatusCode.OK,
                mapper.Map<StablePostDto>(stablePost),
                "Stable post information updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<StablePostDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<StablePostDto?>> DeleteStablePost(int id)
    {
        try
        {
            var stablePost = await context.StablePosts
                .Where(sp => sp.Id == id)
                .FirstOrDefaultAsync();

            if (stablePost == null)
                return ApiResponse<StablePostDto>.Failure(HttpStatusCode.NotFound,
                "Error: Stable post not found");

            context.StablePosts.Remove(stablePost);
            await context.SaveChangesAsync();

            return ApiResponse<StablePostDto>.Success(HttpStatusCode.NoContent,
                null,
                $"Stable post with id '{id}' was deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<StablePostDto>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}