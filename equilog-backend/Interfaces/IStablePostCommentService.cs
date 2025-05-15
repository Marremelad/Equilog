using equilog_backend.Common;

namespace equilog_backend.Interfaces;

public interface IStablePostCommentService
{
    Task<ApiResponse<int>> CreateStablePostCommentConnectionAsync(int stablePostId, int commentId);

    Task<ApiResponse<Unit>> RemoveStablePostCommentConnectionAsync(int stablePostCommentId);
}