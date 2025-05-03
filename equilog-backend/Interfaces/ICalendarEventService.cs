using equilog_backend.Common;
using equilog_backend.DTOs.CalendarEventDTOs;

namespace equilog_backend.Interfaces;

public interface ICalendarEventService
{
    Task<ApiResponse<List<CalendarEventDto>?>> GetCalendarEventsByStableIdAsync(int stableId);
    
    Task<ApiResponse<List<CalendarEventDto>?>> GetCalendarEventsAsync();

    Task<ApiResponse<CalendarEventDto?>> GetCalendarEventAsync(int calendarEventId);
    
    Task<ApiResponse<CalendarEventDto?>> CreateCalendarEventAsync(CalendarEventCreateDto calendarEventCreateDto);

    Task<ApiResponse<Unit>> UpdateCalendarEventAsync(CalendarEventUpdateDto calendarEventUpdateDto);

    Task<ApiResponse<Unit>> DeleteCalendarEventAsync(int calendarEventId);
}