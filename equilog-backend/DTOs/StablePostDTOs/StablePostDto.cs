namespace equilog_backend.DTOs.StablePostDTOs;

public class StablePostDto
{
    public required int Id { get; set; }
    
    public required string Title { get; set; }

    public required string Content { get; set; }

    public required DateTime Date { get; set; }

    public required bool IsPinned { get; set; }
}