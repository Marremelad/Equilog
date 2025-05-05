namespace equilog_backend.DTOs.CalendarEventDTOs;

public class CalendarEventUpdateDto
{
    public required int Id { get; init; }

    public required string Title { get; init; }

    public required DateTime StartDateTime { get; init; }

    public required DateTime EndDateTime { get; init; }
}