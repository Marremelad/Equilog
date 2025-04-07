using System.Net;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class EventService(EquilogDbContext context)
{
    public async Task<ApiResponse<List<EventDto>?>> GetEvents()
    {
        try
        {
            var events = await context.Events
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    StartDateTime = e.StartDateTime,
                    EndDateTime = e.EndDateTime
                })
                .ToListAsync();

            return ApiResponse<List<EventDto>>.Success(HttpStatusCode.OK, events, null);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<EventDto>>.Failure(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}