using equilog_backend.Common;
using equilog_backend.DTOs.CommentDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class CommentEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Create comment.
        app.MapPost("/api/comment/create", CreateComment)
            .WithName("CreateComment");
        
        // Delete comment.
        app.MapDelete("/api/comment/delete/{id:int}", DeleteComment)
            .WithName("DeleteComment");
    }

    private static async Task<IResult> CreateComment(
        ICommentService commentService,
        CommentCreateDto commentCreateDto)
    {
        return Result.Generate(await commentService.CreateCommentAsync(commentCreateDto));
    }

    private static async Task<IResult> DeleteComment(
        ICommentService commentService,
        int id)
    {
        return Result.Generate(await commentService.DeleteCommentAsync(id));
    }
}