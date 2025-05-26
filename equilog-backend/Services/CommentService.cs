using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.CommentDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

// Service that manages comments on stable posts, including retrieval, creation, and deletion.
// Handles the relationship between comments, users, and stable posts through junction tables.
public class CommentService(EquilogDbContext context, IMapper mapper) : ICommentService
{
    // Retrieves all comments associated with a specific stable post.
    public async Task<ApiResponse<List<CommentDto>?>> GetCommentByStablePostId(int stablePostId)
    {
        try
        {
            // Fetch comments that are linked to the specified stable post.
            // Includes user information through the UserComments junction table.
            var commentDtos = mapper.Map<List<CommentDto>>(
                await context.Comments
                .Include(c => c.UserComments)!
                .ThenInclude(uc => uc.User)
                .Where(c => c.StablePostComments != null &&
                            c.StablePostComments.Any(spc => spc.StablePostIdFk == stablePostId))
                .ToListAsync());

            // Provides an appropriate message based on whether comments were found or not.
            var message = commentDtos.Count == 0
                ? "Operation was successful but the post has no comments."
                : "Comments fetched successfully.";
            
            return ApiResponse<List<CommentDto>?>.Success(
                HttpStatusCode.OK,
                commentDtos,
                message);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<CommentDto>>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Creates a new comment and returns its ID for linking to other entities.
    public async Task<ApiResponse<int>> CreateCommentAsync(CommentCreateDto commentCreateDto)
    {
        try
        {
            // Create a new comment entity with the current timestamp and provided content.
            var comment = new Comment
            {
                CommentDate = DateTime.Now,
                Content = commentCreateDto.Content
            };

            // Add the comment to the database and save to generate the ID.
            context.Comments.Add(comment);
            await context.SaveChangesAsync();
            
            // Return the generated comment ID for use in junction table relationships.
            return ApiResponse<int>.Success(
                HttpStatusCode.Created,
                comment.Id, 
                "Comment created successfully.");

        }
        catch (Exception ex)
        {
            return ApiResponse<int>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Removes a comment from the system along with all its relationships.
    public async Task<ApiResponse<Unit>> DeleteCommentAsync(int commentId)
    {
        try
        {
            // Find the comment to delete.
            var comment = await context.Comments
                .Where(c => c.Id == commentId)
                .FirstOrDefaultAsync();
            
            // Returns an error if the comment doesn't exist.
            if (comment == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Comment not found.");

            // Remove the comment (cascade delete will handle junction table cleanup).
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                $"Comment with id {commentId} deleted successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}