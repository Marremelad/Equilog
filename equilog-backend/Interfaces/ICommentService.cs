using equilog_backend.Common;
using equilog_backend.DTOs.CommentDTOs;

namespace equilog_backend.Interfaces;

public interface ICommentService
{
    Task<ApiResponse<List<CommentDto>?>> GetCommentByStablePostId(int stablePostId);
    
    Task<ApiResponse<int>> CreateCommentAsync(CommentCreateDto commentCreateDto);

    Task<ApiResponse<Unit>> DeleteCommentAsync(int commentId);
}