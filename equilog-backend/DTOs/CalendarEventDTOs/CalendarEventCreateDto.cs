namespace equilog_backend.DTOs.CalendarEventDTOs;

public class CalendarEventCreateDto
{
    public required string Title { get; set; }

    public required DateTime StartDateTime { get; set; }

    public required DateTime EndDateTime { get; set; }

    public required int UserIdFk { get; set; }

    public required int StableIdFk { get; set; }
}