using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs;

public class CalendarEventDto
{
    [Required]
    public required int Id { get; set; }
    
    [Required]
    public required string Title { get; set; }

    [Required]
    public required DateTime StartDateTime { get; set; }

    [Required]
    public required DateTime EndDateTime { get; set; }
}