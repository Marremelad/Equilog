namespace equilog_backend.DTOs.StablePostDTOs;

public class StablePostCreateDto
{
    public required int UserIdFk { get; set; }

    public required int StableIdFk { get; set; }

    public required string Title { get; set; }

    public required string Content { get; set; }

    public required DateTime Date { get; set; }

    public required bool IsPinned { get; set; }
}