using equilog_backend.Common;
using equilog_backend.DTOs.CommentCompositionDTOs;
using equilog_backend.DTOs.CommentDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class CommentEndpoints
{
	public static void RegisterEndpoints(WebApplication app)
	{
		// Get comments by StablePost id.
		app.MapGet("/api/comment/{stablePostId:int}", GetCommentsByStablePostId) // "/api/stable-posts/{id:int}/comments"
			.WithName("GetCommentByStableId");

		// Create comment.
		app.MapPost("/api/comment/create", CreateComment) // "/api/comments"
			.AddEndpointFilter<ValidationFilter<CommentCreateDto>>()
			.WithName("CreateComment");

		// Delete comment.
		app.MapDelete("/api/comment/delete/{commentId:int}", DeleteComment) // "/api/comments/{id:int}"
			.WithName("DeleteComment");

		// -- Endpoints for compositions --

		// create a comment with required components and relations.
		app.MapPost("/api/comment/create/composition", CreateCommentComposition) // "/api/comments/compositions"
			.AddEndpointFilter<ValidationFilter<CommentCompositionCreateDto>>()
			.WithName("CreateCommentComposition");
	}

	private static async Task<IResult> GetCommentsByStablePostId(
		ICommentService commentService,
		int stablePostId)
	{
		return Result.Generate(await commentService.GetCommentByStablePostId(stablePostId));
	}

	private static async Task<IResult> CreateComment(
		ICommentService commentService,
		CommentCreateDto commentCreateDto)
	{
		return Result.Generate(await commentService.CreateCommentAsync(commentCreateDto));
	}

	private static async Task<IResult> DeleteComment(
		ICommentService commentService,
		int commentId)
	{
		return Result.Generate(await commentService.DeleteCommentAsync(commentId));
	}

	// -- Result generators for composition --
	private static async Task<IResult> CreateCommentComposition(
		ICommentComposition commentComposition,
		CommentCompositionCreateDto commentCompositionCreateDto)
	{
		return Result.Generate(await commentComposition.CreateCommentComposition(commentCompositionCreateDto));
	}
}