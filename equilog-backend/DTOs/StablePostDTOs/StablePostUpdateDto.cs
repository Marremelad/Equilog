using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.StablePostDTOs;

public class StablePostUpdateDto
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Content { get; set; }
}