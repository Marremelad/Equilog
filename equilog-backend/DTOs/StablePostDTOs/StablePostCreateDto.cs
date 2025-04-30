namespace equilog_backend.DTOs.StablePostDTOs;

public class StablePostCreateDto
{
    public required int UserIdFk { get; init; }

    public required int StableIdFk { get; init; }

    public required string Title { get; init; }

    public required string Content { get; init; }

    public required DateTime Date { get; init; }

    public required bool IsPinned { get; init; }
}