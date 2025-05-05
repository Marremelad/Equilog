namespace equilog_backend.DTOs.StablePostDTOs;

public class StablePostDto
{
    public required int Id { get; set; }

    public required string PosterFirstName { get; init; }

    public required string PosterLastName { get; init; }

    public required string Title { get; init; }

    public required string Content { get; init; }

    public required DateTime Date { get; init; }

    public required bool IsPinned { get; init; }
}