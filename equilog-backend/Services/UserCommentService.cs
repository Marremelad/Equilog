using System.Net;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

// Service that manages the relationship between users and comments.
// Handles the junction table that links comments to their authors and manages comment ownership.
public class UserCommentService(EquilogDbContext context) : IUserCommentService
{
    // Creates a relationship between a user and a comment (establishes comment authorship).
    public async Task<ApiResponse<Unit>> CreateUserCommentConnectionAsync(int userId, int commentId)
    {
        try
        {
            // Create the user-comment relationship entity to establish authorship.
            var userComment = new UserComment
            {
                UserIdFk = userId,
                CommentIdFk = commentId
            };

            // Add the relationship to the database and save.
            context.UserComments.Add(userComment);
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(
                HttpStatusCode.Created,
                Unit.Value,
                "Connection between user and comment was created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError, 
                ex.Message);
        }
    }

    // Removes the relationship between a user and a comment.
    public async Task<ApiResponse<Unit>> RemoveUserCommentConnection(int userCommentId)
    {
        try
        {
            // Find the user-comment relationship to remove.
            var userComment = await context.UserComments
                .Where(uc => uc.Id == userCommentId)
                .FirstOrDefaultAsync();
            
            // Returns an error if the relationship doesn't exist.
            if (userComment == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound, 
                    "Error: Connection between user and comment not found.");
            
            // Remove the relationship from the database.
            context.UserComments.Remove(userComment);
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "Connection between user and comment was removed successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}