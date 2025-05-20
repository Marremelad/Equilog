using System.Net;
using equilog_backend.Common;
using equilog_backend.DTOs.CommentCompositionDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Compositions;

public class CommentComposition(
    ICommentService commentService,
    IUserCommentService userCommentService,
    IStablePostCommentService stablePostCommentService) : ICommentComposition
{
    public async Task<ApiResponse<Unit>> CreateCommentComposition(
        CommentCompositionCreateDto commentCompositionCreateDto)
    {
        var commentResponse = await commentService.CreateCommentAsync(commentCompositionCreateDto.Comment);
        
        if (!commentResponse.IsSuccess)
            return ApiResponse<Unit>.Failure(commentResponse.StatusCode,
                $"Failed to create comment: {commentResponse.Message}");

        var commentId = commentResponse.Value;
        var userId = commentCompositionCreateDto.UserId;
        var stablePostId = commentCompositionCreateDto.StablePostId;

        var userCommentResponse = await userCommentService.CreateUserCommentConnectionAsync(userId, commentId);

        if (!userCommentResponse.IsSuccess)
        {
            await commentService.DeleteCommentAsync(commentId);
            return ApiResponse<Unit>.Failure(userCommentResponse.StatusCode,
                $"{userCommentResponse.Message}: Comment creation was rolled back.");
        }

        var userCommentId = userCommentResponse.Value;

        var stablePostCommentResponse =
            await stablePostCommentService.CreateStablePostCommentConnectionAsync(stablePostId, commentId);

        if (!stablePostCommentResponse.IsSuccess)
        {
            await commentService.DeleteCommentAsync(commentId);
            await userCommentService.RemoveUserCommentConnection(userCommentId);
            return ApiResponse<Unit>.Failure(stablePostCommentResponse.StatusCode,
                $"{stablePostCommentResponse.Message}: Comment creation and connection between user and comment was rolled back.");
        }
        
        return ApiResponse<Unit>.Success(HttpStatusCode.Created,
            Unit.Value,
            null);
    }
}