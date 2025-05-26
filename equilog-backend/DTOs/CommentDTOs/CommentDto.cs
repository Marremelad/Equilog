namespace equilog_backend.DTOs.CommentDTOs;

public class CommentDto
{
    public required int Id { get; init; }
    
    public required DateTime CommentDate { get; init; }
    
    public required string Content { get; init; }

    public required int UserId { get; init; }
    
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }
    
    public required string ProfilePicture { get; init; }
}