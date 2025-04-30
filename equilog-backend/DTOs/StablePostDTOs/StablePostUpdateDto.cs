namespace equilog_backend.DTOs.StablePostDTOs;

public class StablePostUpdateDto
{
    public required int Id { get; init; }

    public required string Title { get; init; }

    public required string Content { get; init; }
}