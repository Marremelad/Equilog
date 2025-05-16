using equilog_backend.DTOs.CommentDTOs;

namespace equilog_backend.DTOs.CommentCompositionDTOs;

public class CommentCompositionCreateDto
{
    public required int UserId { get; set; }
    
    public required int StablePostId { get; set; }
    
    public required CommentCreateDto Comment { get; set; }
}