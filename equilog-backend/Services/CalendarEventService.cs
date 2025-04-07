using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class CalendarEventService(EquilogDbContext context, IMapper mapper)
{
    public async Task<ApiResponse<List<EventDto>?>> GetEvents()
    {
        try
        {
            var eventDto = mapper.Map<List<EventDto>>(await context.CalendarEvents.ToListAsync());

            return ApiResponse<List<EventDto>>.Success(HttpStatusCode.OK, eventDto, null);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<EventDto>>.Failure(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}