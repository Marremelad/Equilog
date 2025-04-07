using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.CalendarEventDTOs;

public class CalendarEventCreateDto
{
    [Required]
    [StringLength(50)]
    public required string Title { get; set; }

    [Required]
    public required DateTime StartDateTime { get; set; }

    [Required]
    public required DateTime EndDateTime { get; set; }

    [Required]
    public int StableIdFk { get; set; }
}