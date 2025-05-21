using equilog_backend.Common;
using equilog_backend.DTOs.CalendarEventDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public abstract class CalendarEventEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get calendar events by stable id.
        app.MapGet("/api/calendar-events/{id:int}", GetCalendarEventsByStableId)
            .WithName("GetCalendarEventsByStableId")
            .RequireAuthorization();
        
        // Get calendar event by id.
        app.MapGet("/api/calendar-event/{id:int}", GetCalendarEvent)
            .WithName("GetCalendarEvent")
            .RequireAuthorization();

        // Create calendar event.
        app.MapPost("/api/calendar-event/create", CreateCalendarEvent)
            .AddEndpointFilter<ValidationFilter<CalendarEventCreateDto>>()
            .WithName("CreateCalendarEvent");
            // .RequireAuthorization();

        // Update calendar event.
        app.MapPut("/api/calendar-event/update", UpdateCalendarEvent)
            .AddEndpointFilter<ValidationFilter<CalendarEventUpdateDto>>()
            .WithName("UpdateCalendarEvent")
            .RequireAuthorization();

        // Delete calendar event.
        app.MapDelete("/api/calendar-event/delete/{id:int}", DeleteCalendarEvent)
            .WithName("DeleteCalendarEvent")
            .RequireAuthorization();
        
        // Get all calendar events.
        app.MapGet("/api/calendar-events", GetCalendarEvents)
            .WithName("GetCalendarEvents")
            .RequireAuthorization();
    }
    
    private static async Task<IResult> GetCalendarEventsByStableId(
        ICalendarEventService calendarEventService,
        int id)
    {
        return Result.Generate(await calendarEventService.GetCalendarEventsByStableIdAsync(id));
    }
    
    private static async Task<IResult> GetCalendarEvent(
        ICalendarEventService calendarEventService,
        int id)
    {
        return Result.Generate(await calendarEventService.GetCalendarEventAsync(id));
    }

    private static async Task<IResult> CreateCalendarEvent(
        ICalendarEventService calendarEventService,
        CalendarEventCreateDto newCalendarEvent)
    {
        return Result.Generate(await calendarEventService.CreateCalendarEventAsync(newCalendarEvent));
    }

    private static async Task<IResult> UpdateCalendarEvent(
        ICalendarEventService calendarEventService,
        CalendarEventUpdateDto updatedEvent)
    {
        return Result.Generate(await calendarEventService.UpdateCalendarEventAsync(updatedEvent));
    }

    private static async Task<IResult> DeleteCalendarEvent(
        ICalendarEventService calendarEventService,
        int id)
    {
        return Result.Generate(await calendarEventService.DeleteCalendarEventAsync(id));
    }
    
    private static async Task<IResult> GetCalendarEvents(
        ICalendarEventService calendarEventService)
    {
        return Result.Generate(await calendarEventService.GetCalendarEventsAsync());
    }
}