namespace equilog_backend.DTOs.StablePostDTOs;

public class StablePostUpdateDto
{
    public required int Id { get; set; }

    public required string Title { get; set; }

    public required string Content { get; set; }
}