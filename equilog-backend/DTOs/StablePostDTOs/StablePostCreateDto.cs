using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.StablePostDTOs;

public class StablePostCreateDto
{
    [Required]
    public required int UserIdFk { get; set; }

    [Required]
    public required int StableIdFk { get; set; }

    [Required]
    [StringLength(510)]
    public required string Title { get; set; }

    [Required]
    [StringLength(4094)]
    public required string Content { get; set; }

    [Required]
    public required DateTime Date { get; set; }

    [Required]
    public required bool IsPinned { get; set; }
}