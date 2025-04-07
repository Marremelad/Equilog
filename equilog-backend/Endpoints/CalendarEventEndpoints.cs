using System.Net;
using equilog_backend.DTOs.CalendarEventDTOs;
using equilog_backend.Services;

namespace equilog_backend.Endpoints;

public class CalendarEventEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get all calendar events.
        app.MapGet("/api/calendar-event", GetEvents)
            .WithName("GetEvents");

        // Get calendar event by id.
        app.MapGet("/api/calendar-event/{id:int}", GetEvent)
            .WithName("GetEvent");

        // Create calendar event.
        app.MapPost("/api/calendar-event/create", CreateCalendarEvent)
            .WithName("CreateCalendarEvent");

        // Update calendar event.
        app.MapPut("/api/calendar-event/update", UpdateEvent)
            .WithName("UpdateEvent");
        
        // Delete calendar event.
        app.MapDelete("/api/calendar-event/delete/{id:int}", DeleteCalendarEvent)
            .WithName("DeleteCalendarEvent");
    }

    private static async Task<IResult> GetEvents(CalendarEventService calendarEventService)
    {
        var apiResponse = await calendarEventService.GetEvents();

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
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
    
    private static async Task<IResult> CreateCalendarEvent(CalendarEventService calendarEventService,
        CalendarEventCreateDto newCalendarEvent)
    {
        var apiResponse = await calendarEventService.CreateCalendarEvent(newCalendarEvent);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.Created => Results.Json(apiResponse, statusCode: 201),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> UpdateEvent(CalendarEventService calendarEventService,
        CalendarEventUpdateDto updatedEvent)
    {
        var apiResponse = await calendarEventService.UpdateCalendarEvent(updatedEvent);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> DeleteCalendarEvent(CalendarEventService calendarEventService, int id)
    {
        var apiResponse = await calendarEventService.DeleteCalendarEvent(id);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.NoContent => Results.Json(apiResponse, statusCode: 204),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }
}