using equilog_backend.DTOs.UserDTOs;

namespace equilog_backend.DTOs.StablePostDTOs;

public class StablePostDto
{
    public required UserDto User { get; init; }

    public required string Title { get; init; }

    public required string Content { get; init; }

    public required DateTime Date { get; init; }

    public required bool IsPinned { get; init; }
}