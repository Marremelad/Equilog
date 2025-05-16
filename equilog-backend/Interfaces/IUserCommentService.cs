using equilog_backend.Common;

namespace equilog_backend.Interfaces;

public interface IUserCommentService
{
    Task<ApiResponse<int>> CreateUserCommentConnectionAsync(int userId, int commentId);

    Task<ApiResponse<Unit>> RemoveUserCommentConnection(int userCommentId);
}