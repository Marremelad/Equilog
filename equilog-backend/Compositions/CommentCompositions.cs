using System.Net;
using equilog_backend.Common;
using equilog_backend.DTOs.CommentCompositionDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Compositions;

// Composition service that orchestrates the creation of a comment with all required relationships.
// Handles the complex operation of creating a comment and linking it to both user and stable post.
public class CommentCompositions(
    ICommentService commentService,
    IUserCommentService userCommentService,
    IStablePostCommentService stablePostCommentService) : ICommentComposition
{
    // Creates a complete comment composition including the comment and all its relationships.
    public async Task<ApiResponse<Unit>> CreateCommentComposition(
        CommentCompositionCreateDto commentCompositionCreateDto)
    {
        try
        {
            // Step 1: Create the core comment entity.
            var commentResponse = await commentService.CreateCommentAsync(commentCompositionCreateDto.Comment);
        
            // If comment creation fails, return immediately without creating relationships.
            if (!commentResponse.IsSuccess)
                return ApiResponse<Unit>.Failure(
                    commentResponse.StatusCode,
                    $"Failed to create comment: {commentResponse.Message}");

            // Extract IDs for creating the relationships.
            var commentId = commentResponse.Value;
            var userId = commentCompositionCreateDto.UserId;
            var stablePostId = commentCompositionCreateDto.StablePostId;

            // Step 2: Create the user-comment relationship.
            var userCommentResponse = await userCommentService.CreateUserCommentConnectionAsync(userId, commentId);

            // If the user-comment relationship fails, rollback by deleting the comment.
            if (!userCommentResponse.IsSuccess)
            {
                await commentService.DeleteCommentAsync(commentId);
                userCommentResponse.Message =
                    $"Failed to create connection between user and comment: {userCommentResponse.Message}. Comment creation was rolled back.";
                return userCommentResponse;
            }
        
            // Step 3: Create a stable post-comment relationship.
            var stablePostCommentResponse =
                await stablePostCommentService.CreateStablePostCommentConnectionAsync(stablePostId, commentId);

            // If the stable post-comment relationship fails, rollback by deleting the comment.
            if (!stablePostCommentResponse.IsSuccess)
            {
                await commentService.DeleteCommentAsync(commentId);
                stablePostCommentResponse.Message =
                    $"Failed to create connection between stable-post and comment: {userCommentResponse.Message}. Comment creation was rolled back.";
                return stablePostCommentResponse;
            }
        
            // All operations successful - return success response.
            return ApiResponse<Unit>.Success(
                HttpStatusCode.Created,
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