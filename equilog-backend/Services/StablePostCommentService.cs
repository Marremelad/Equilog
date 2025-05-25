using System.Net;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class StablePostCommentService(EquilogDbContext context) : IStablePostCommentService
{
    public async Task<ApiResponse<Unit>> CreateStablePostCommentConnectionAsync(int stablePostId, int commentId)
    {
        try
        {
            var stablePostComment = new StablePostComment
            {
                StablePostIdFk = stablePostId,
                CommentIdFk = commentId
            };

            context.StablePostComments.Add(stablePostComment);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value, 
                "Connection between stable-post and comment created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public async Task<ApiResponse<Unit>> RemoveStablePostCommentConnectionAsync(int stablePostCommentId)
    {
        try
        {
            var stablePostComment = await context.StablePostComments
                .Where(spc => spc.Id == stablePostCommentId)
                .FirstOrDefaultAsync();
            
            if (stablePostComment == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Connection between stable-post and Comment not found");

            context.StablePostComments.Remove(stablePostComment);
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(
                HttpStatusCode.NoContent,
                Unit.Value,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}