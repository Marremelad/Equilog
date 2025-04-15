using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.CalendarEventDTOs;

public class CalendarEventUpdateDto
{
    [Required]
    public required int Id { get; set; }
    
    [Required]
    public required string Title { get; set; }
    
    [Required] 
    public required DateTime StartDateTime { get; set; }

    [Required]
    public required DateTime EndDateTime { get; set; }

    [Required]
    public required int StableIdFk { get; set; }
}