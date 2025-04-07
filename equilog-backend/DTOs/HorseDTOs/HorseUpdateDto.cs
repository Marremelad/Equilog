using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.HorseDTOs;

public class HorseUpdateDto
{
    [Required]
    public required int Id { get; set; }
        
    [Required]
    public required string Name { get; set; }
}