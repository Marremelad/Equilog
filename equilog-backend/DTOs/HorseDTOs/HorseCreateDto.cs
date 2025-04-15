namespace equilog_backend.DTOs.HorseDTOs;

public class HorseCreateDto
{
    public required string Name { get; set; }

    public DateTime? Age { get; set; }

    public string? Color { get; set; }

    public string? Breed { get; set; }
}