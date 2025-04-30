namespace equilog_backend.DTOs.CalendarEventDTOs;

public class CalendarEventCreateDto
{
    public required string Title { get; init; }

    public required DateTime StartDateTime { get; init; }

    public required DateTime EndDateTime { get; init; }

    public required int UserIdFk { get; set; }

    public required int StableIdFk { get; init; }
}