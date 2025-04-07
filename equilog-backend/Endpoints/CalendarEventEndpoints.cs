using System.Net;
using equilog_backend.Common;
using equilog_backend.Common.Enums;
using equilog_backend.DTOs;
using equilog_backend.Services;

namespace equilog_backend.Endpoints;

public class CalendarEventEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/events", GetEvents)
            .WithName("GetEvents")
            .Produces<ApiResponse<List<CalendarEventDto>>>()
            .Produces(500);
    }

    private static async Task<IResult> GetEvents(CalendarEventService calendarEventService)
    {
        var response = await calendarEventService.GetEvents();

        return response.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(response.Value),
            _ => Results.Problem(response.Message, statusCode: 500)
        };
    }
}