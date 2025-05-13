using System.Net;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.CommentDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;

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
}