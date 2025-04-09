using equilog_backend.Common;
using equilog_backend.DTOs.CalendarEventDTOs;

namespace equilog_backend.Interfaces;

public interface ICalendarEventService
{
    public Task<ApiResponse<List<CalendarEventDto>?>> GetCalendarEvents();

    public Task<ApiResponse<CalendarEventDto?>> GetCalendarEvent(int id);
    
    public Task<ApiResponse<CalendarEventDto?>> CreateCalendarEvent(CalendarEventCreateDto newCalendarEvent);

    public Task<ApiResponse<CalendarEventDto?>> UpdateCalendarEvent(CalendarEventUpdateDto updatedCalendarEvent);

    public Task<ApiResponse<CalendarEventDto?>> DeleteCalendarEvent(int id);
}