using equilog_backend.Common;
using equilog_backend.DTOs.CalendarEventDTOs;

namespace equilog_backend.Interfaces;

public interface ICalendarEventService
{
    public Task<ApiResponse<List<CalendarEventDto>?>> GetCalendarEventsByStableIdAsync(int id);
    
    public Task<ApiResponse<List<CalendarEventDto>?>> GetCalendarEventsAsync();

    public Task<ApiResponse<CalendarEventDto?>> GetCalendarEventAsync(int id);
    
    public Task<ApiResponse<CalendarEventDto?>> CreateCalendarEventAsync(CalendarEventCreateDto calendarEventCreateDto);

    public Task<ApiResponse<CalendarEventDto?>> UpdateCalendarEventAsync(CalendarEventUpdateDto calendarEventUpdateDto);

    public Task<ApiResponse<CalendarEventDto?>> DeleteCalendarEventAsync(int id);
}