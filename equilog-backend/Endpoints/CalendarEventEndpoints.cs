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
        var apiResponse = await calendarEventService.GetEvents();

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse.Value),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> GetEvent(CalendarEventService calendarEventService, int id)
    {
        var apiResponse = await calendarEventService.GetEvent(id);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }
}