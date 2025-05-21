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
            var createComment = await commentService.CreateCommentAsync(commentCompositionCreateDto.Comment);
        
            if (!createComment.IsSuccess)
                return ApiResponse<Unit>.Failure(createComment.StatusCode,
                    $"Failed to create comment: {createComment.Message}");

            var commentId = createComment.Value;
            var userId = commentCompositionCreateDto.UserId;
            var stablePostId = commentCompositionCreateDto.StablePostId;

            var createUserComment = await userCommentService.CreateUserCommentConnectionAsync(userId, commentId);

            if (!createUserComment.IsSuccess)
            {
                await commentService.DeleteCommentAsync(commentId);
                return createUserComment;
            }
        
            var createStablePostComment =
                await stablePostCommentService.CreateStablePostCommentConnectionAsync(stablePostId, commentId);

            if (!createStablePostComment.IsSuccess)
            {
                await commentService.DeleteCommentAsync(commentId);
                return createStablePostComment;
            }
        
            return ApiResponse<Unit>.Success(HttpStatusCode.Created,
                Unit.Value,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}