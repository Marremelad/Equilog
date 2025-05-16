namespace equilog_backend.DTOs.CommentDTOs;

public class CommentDto
{
    public required int Id { get; set; }
    
    public required DateTime CommentDate { get; set; }
    
    public required string Content { get; set; }
}