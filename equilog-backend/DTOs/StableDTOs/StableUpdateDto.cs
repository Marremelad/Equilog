using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.StableDTOs;

public class StableUpdateDto
{
    [Required]
    public required int Id { get; set; }

    [Required]
    public required string Name { get; set; }
}