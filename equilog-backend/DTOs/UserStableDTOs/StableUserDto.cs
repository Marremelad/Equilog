namespace equilog_backend.DTOs.UserStableDTOs;

public class StableUserDto
{
    public required int UserStableId { get; init; }

    public required int UserId { get; init; }

    public required int Role { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }
}