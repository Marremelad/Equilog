using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class CalendarEventService(EquilogDbContext context, IMapper mapper)
{
    public async Task<ApiResponse<List<CalendarEventDto>?>> GetEvents()
    {
        try
        {
            var calendarEventDto = mapper.Map<List<CalendarEventDto>>(await context.CalendarEvents.ToListAsync());

            return ApiResponse<List<CalendarEventDto>>.Success(HttpStatusCode.OK, calendarEventDto,
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<CalendarEventDto>>.Failure(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
    
    public async Task<ApiResponse<CalendarEventDto?>> GetEvent(int id)
    {
        try
        {
            var calendarEvent = await context.CalendarEvents
                .Where(ce => ce.Id == id)
                .FirstOrDefaultAsync();

            if (calendarEvent == null) return ApiResponse<CalendarEventDto>.Failure(HttpStatusCode.NotFound,
                    "Error: Event not found");

            return ApiResponse<CalendarEventDto>.Success(HttpStatusCode.OK, mapper.Map<CalendarEventDto>(calendarEvent),
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<CalendarEventDto>.Failure(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
