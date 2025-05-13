using equilog_backend.Common;
using equilog_backend.DTOs.CommentDTOs;

namespace equilog_backend.Interfaces;

public interface ICommentService
{
    Task<ApiResponse<int>> CreateCommentAsync(CommentCreateDto commentCreateDto);
}