using System.Net;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

// Service that manages the relationship between stable posts and comments.
// Handles the junction table that links comments to specific stable posts.
public class StablePostCommentService(EquilogDbContext context) : IStablePostCommentService
{
    // Creates a relationship between a stable post and a comment (links comment to post).
    public async Task<ApiResponse<Unit>> CreateStablePostCommentConnectionAsync(int stablePostId, int commentId)
    {
        try
        {
            // Create a stable post-comment relationship entity.
            var stablePostComment = new StablePostComment
            {
                StablePostIdFk = stablePostId,
                CommentIdFk = commentId
            };

            // Add the relationship to the database and save.
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

    // Removes the relationship between a stable post and a comment.
    public async Task<ApiResponse<Unit>> RemoveStablePostCommentConnectionAsync(int stablePostCommentId)
    {
        try
        {
            // Find the stable post-comment relationship to remove.
            var stablePostComment = await context.StablePostComments
                .Where(spc => spc.Id == stablePostCommentId)
                .FirstOrDefaultAsync();
            
            // Returns an error if the relationship doesn't exist.
            if (stablePostComment == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Connection between stable-post and Comment not found");

            // Remove the relationship from the database.
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