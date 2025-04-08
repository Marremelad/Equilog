using equilog_backend.Common;
using equilog_backend.DTOs.CalendarEventDTOs;

namespace equilog_backend.Interfaces;

public interface ICalendarEventService
{
    public Task<ApiResponse<List<CalendarEventDto>?>> GetEvents();

    public Task<ApiResponse<CalendarEventDto?>> GetEvent(int id);
    
    public Task<ApiResponse<CalendarEventDto?>> CreateCalendarEvent(CalendarEventCreateDto newCalendarEvent);

    public Task<ApiResponse<CalendarEventDto?>> UpdateCalendarEvent(CalendarEventUpdateDto updatedCalendarEvent);

    public Task<ApiResponse<CalendarEventDto?>> DeleteCalendarEvent(int id);
}