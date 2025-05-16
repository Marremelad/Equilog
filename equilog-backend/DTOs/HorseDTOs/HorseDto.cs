using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.HorseDTOs;

public class HorseDto
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public DateOnly? Age { get; init; }
        
    public string? Color { get; init; }

    public string? Breed { get; init; }

    public string? CoreInformation { get; set; }

    public string? Description { get; set; }

    public int? Weight { get; set; }

    public int? Height { get; set; }

    public int? CurrentBox { get; set; }
}