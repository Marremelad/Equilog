using System.Net;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<ApiResponse<Unit>> RemoveUserCommentConnection(int userCommentId)
    {
        try
        {
            var userComment = await context.UserComments
                .Where(uc => uc.Id == userCommentId)
                .FirstOrDefaultAsync();
            
            if (userComment == null)
                return ApiResponse<Unit>.Failure(HttpStatusCode.NotFound,
                    "Error: Connection between user and comment not found.");
            
            context.UserComments.Remove(userComment);
            await context.SaveChangesAsync();
            
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                "Connection between user and comment was removed successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}