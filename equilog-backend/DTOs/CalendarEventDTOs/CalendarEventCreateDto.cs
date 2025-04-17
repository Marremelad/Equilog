namespace equilog_backend.DTOs.CalendarEventDTOs;

public class CalendarEventCreateDto
{
    public required string Title { get; set; }

    public required DateTime StartDateTime { get; set; }

    public required DateTime EndDateTime { get; set; }

    public int StableIdFk { get; set; }
}