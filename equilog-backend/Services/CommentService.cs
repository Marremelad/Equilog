using System.Net;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.CommentDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class CommentService(EquilogDbContext context) : ICommentService
{
    public async Task<ApiResponse<int>> CreateCommentAsync(CommentCreateDto commentCreateDto)
    {
        try
        {
            var comment = new Comment
            {
                CommentDate = DateTime.Now,
                Content = commentCreateDto.Content
            };

            context.Comments.Add(comment);
            await context.SaveChangesAsync();
            
            return ApiResponse<int>.Success(HttpStatusCode.Created,
                comment.Id,
                "Comment created successfully.");

        }
        catch (Exception ex)
        {
            return ApiResponse<int>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> DeleteCommentAsync(int commentId)
    {
        try
        {
            var comment = await context.Comments
                .Where(c => c.Id == commentId)
                .FirstOrDefaultAsync();
            
            if (comment == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Error: Comment not found");

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                "Comment deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}