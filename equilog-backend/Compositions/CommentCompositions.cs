using System.Net;
using equilog_backend.Common;
using equilog_backend.DTOs.CommentCompositionDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Compositions;

public class CommentCompositions(
    ICommentService commentService,
    IUserCommentService userCommentService,
    IStablePostCommentService stablePostCommentService) : ICommentComposition
{
    public async Task<ApiResponse<Unit>> CreateCommentComposition(
        CommentCompositionCreateDto commentCompositionCreateDto)
    {
        try
        {
            var commentResponse = await commentService.CreateCommentAsync(commentCompositionCreateDto.Comment);
        
            if (!commentResponse.IsSuccess)
                return ApiResponse<Unit>.Failure(
                    commentResponse.StatusCode,
                    $"Failed to create comment: {commentResponse.Message}");

            var commentId = commentResponse.Value;
            var userId = commentCompositionCreateDto.UserId;
            var stablePostId = commentCompositionCreateDto.StablePostId;

            var userCommentResponse = await userCommentService.CreateUserCommentConnectionAsync(userId, commentId);

            if (!userCommentResponse.IsSuccess)
            {
                await commentService.DeleteCommentAsync(commentId);
                userCommentResponse.Message =
                    $"Failed to create connection between user and comment: {userCommentResponse.Message}. Comment creation was rolled back.";
                return userCommentResponse;
            }
        
            var stablePostCommentResponse =
                await stablePostCommentService.CreateStablePostCommentConnectionAsync(stablePostId, commentId);

            if (!stablePostCommentResponse.IsSuccess)
            {
                await commentService.DeleteCommentAsync(commentId);
                stablePostCommentResponse.Message =
                    $"Failed to create connection between stable-post and comment: {userCommentResponse.Message}. Comment creation was rolled back.";
                return stablePostCommentResponse;
            }
        
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