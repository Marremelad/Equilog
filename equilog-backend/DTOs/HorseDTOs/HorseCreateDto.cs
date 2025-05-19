namespace equilog_backend.DTOs.HorseDTOs;

public class HorseCreateDto
{
    public required string Name { get; init; }

    public DateOnly? Age { get; init; }

    public string? Color { get; init; }

    public string? Breed { get; init; }

    public string? ProfilePicture { get; set; }
}