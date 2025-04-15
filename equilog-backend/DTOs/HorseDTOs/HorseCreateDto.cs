using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.HorseDTOs;

public class HorseCreateDto
{
    [Required]
    [StringLength(50)]
    public required string Name { get; set; }

    public DateOnly? Age { get; set; }
    
    [StringLength(50)]
    public string? Color { get; set; }

    [StringLength(50)]
    public string? Breed { get; set; }
}