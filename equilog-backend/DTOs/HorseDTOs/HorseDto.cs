namespace equilog_backend.DTOs.HorseDTOs;

public class HorseDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public DateOnly? Age { get; set; }
        
    public string? Color { get; set; }

    public string? Breed { get; set; }
}