namespace equilog_backend.DTOs.HorseDTOs;

public class HorseDto
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public DateOnly? Age { get; init; }
        
    public string? Color { get; init; }

    public string? Breed { get; init; }
}