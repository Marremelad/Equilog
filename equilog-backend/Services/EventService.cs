using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class EventService(EquilogDbContext context)
{
    public async Task<Response<List<EventDto>?>> GetAllEvents()
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

            return Response<List<EventDto>>.Success(events);
        }
        catch (Exception ex)
        {
            return Response<List<EventDto>>.Failure(ex.Message);
        }
    }
}