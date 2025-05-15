using System.Net;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.Interfaces;
using equilog_backend.Models;

namespace equilog_backend.Services;

public class StablePostCommentService(EquilogDbContext context) : IStablePostCommentService
{
    public async Task<ApiResponse<int>> CreateStablePostCommentConnectionAsync(int stablePostId, int commentId)
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

            return ApiResponse<int>.Success(HttpStatusCode.OK,
                stablePostComment.Id,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<int>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    
}