using System.Net;
using equilog_backend.DTOs.CalendarEventDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class CalendarEventEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get all calendar events.
        app.MapGet("/api/calendar-event", GetCalendarEvents)
            .WithName("GetCalendarEvents");

        // Get calendar event by id.
        app.MapGet("/api/calendar-event/{id:int}", GetCalendarEvent)
            .WithName("GetCalendarEvent");

        // Create calendar event.
        app.MapPost("/api/calendar-event/create", CreateCalendarEvent)
            .WithName("CreateCalendarEvent");

        // Update calendar event.
        app.MapPut("/api/calendar-event/update", UpdateCalendarEvent)
            .WithName("UpdateCalendarEvent");
        
        // Delete calendar event.
        app.MapDelete("/api/calendar-event/delete/{id:int}", DeleteCalendarEvent)
            .WithName("DeleteCalendarEvent");
    }

    private static async Task<IResult> GetCalendarEvents(ICalendarEventService calendarEventService)
    {
        var apiResponse = await calendarEventService.GetCalendarEvents();

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> GetCalendarEvent(ICalendarEventService calendarEventService, int id)
    {
        var apiResponse = await calendarEventService.GetCalendarEvent(id);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }
    
    private static async Task<IResult> CreateCalendarEvent(ICalendarEventService calendarEventService,
        CalendarEventCreateDto newCalendarEvent)
    {
        var apiResponse = await calendarEventService.CreateCalendarEvent(newCalendarEvent);

        return apiResponse.StatusCode switch
        {
            HttpStatusCode.Created => Results.Json(apiResponse, statusCode: 201),
            _ => Results.Problem(apiResponse.Message, statusCode: 500)
        };
    }

    private static async Task<IResult> UpdateCalendarEvent(ICalendarEventService calendarEventService,
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

    private static async Task<IResult> DeleteCalendarEvent(ICalendarEventService calendarEventService, int id)
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