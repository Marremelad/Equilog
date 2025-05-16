using equilog_backend.Common;
using equilog_backend.DTOs.CommentCompositionDTOs;

namespace equilog_backend.Interfaces;

public interface ICommentComposition
{
    Task<ApiResponse<Unit>> CreateCommentComposition(CommentCompositionCreateDto commentCompositionCreateDto);
}