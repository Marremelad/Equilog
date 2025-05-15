using equilog_backend.Interfaces;
using equilog_backend.Services;

namespace equilog_backend.Compositions;

public class CommentComposition(
    ICommentService commentService,
    IUserCommentService userCommentService,
    IStablePostCommentService stablePostCommentService) : ICommentComposition
{
    
}