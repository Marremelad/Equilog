namespace equilog_backend.DTOs.HorseCompositionDTOs;

public class HorseCompositionCreateDto
{
    public required int UserId { get; init; }
    
    public required int StableId { get; set; }
    
    public required string Name { get; init; }

    public DateOnly? Age { get; init; }

    public string? Color { get; init; }

    public string? Breed { get; init; }
}