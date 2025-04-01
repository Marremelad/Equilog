using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

public class EventService(EquilogDbContext context)
{
    public async Task<Response<List<Event>?>> GetAllEvents()
    {
        try
        {
            var events = await context.Events
                .ToListAsync();

            return Response<List<Event>>.Success(events);
        }
        catch (Exception ex)
        {
            return Response<List<Event>>.Failure(ex.Message);
        }
    }
}