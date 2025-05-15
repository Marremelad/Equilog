using System.Net;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.Interfaces;
using equilog_backend.Models;

namespace equilog_backend.Services;

public class UserCommentService(EquilogDbContext context) : IUserCommentService
{
    public async Task<ApiResponse<int>> CreateUserCommentConnectionAsync(int userId, int commentId)
    {
        try
        {
            var userComment = new UserComment
            {
                UserIdFk = userId,
                CommentIdFk = commentId
            };

            context.UserComments.Add(userComment);
            await context.SaveChangesAsync();
            
            return ApiResponse<int>.Success(HttpStatusCode.Created,
                userComment.Id,
                null);

        }
        catch (Exception ex)
        {
            return ApiResponse<int>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}