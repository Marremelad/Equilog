namespace equilog_backend.DTOs.HorseDTOs;

public class HorseDto
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public DateOnly? Age { get; init; }
        
    public string? Color { get; init; }

    public string? Breed { get; init; }

    public string? CoreInformation { get; init; }

    public string? Description { get; init; }

    public int? Weight { get; init; }

    public int? Height { get; init; }

    public int? CurrentBox { get; init; }

    public string? ProfilePicture { get; set; }
}