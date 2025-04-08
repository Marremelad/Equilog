using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.StableDTOs;

public class StableCreateDto
{
    [Required]
    public required string Name { get; set; }
}