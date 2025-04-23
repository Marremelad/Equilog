namespace equilog_backend.DTOs.CalendarEventDTOs;

public class CalendarEventDto
{
    public required int Id { get; set; }
    
    public required string Title { get; set; }

    public required DateTime StartDateTime { get; set; }

    public required DateTime EndDateTime { get; set; }

    public required int UserIdFk { get; set; }
}