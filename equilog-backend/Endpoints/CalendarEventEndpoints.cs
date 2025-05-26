using equilog_backend.Common;
using equilog_backend.DTOs.CalendarEventDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public abstract class CalendarEventEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Suppressing ASP0022: RESTful routes with the same path but different HTTP methods are valid
        // #pragma warning disable ASP0022
        
        // Get calendar events by stable id.
        app.MapGet("/api/calendar-events/{stableId:int}", GetCalendarEventsByStableId) // "/api/stables/{stableId:int}/calendar-events"
            .RequireAuthorization()
            .WithName("GetCalendarEventsByStableId");
        
        // Get calendar event by user id.
        app.MapGet("/api/calendar-event/{calendarEventId:int}", GetCalendarEvent) // "/api/calendar-events/{calendarEventId:int}"
            .RequireAuthorization()
            .WithName("GetCalendarEvent");

        // Create calendar event.
        app.MapPost("/api/calendar-event/create", CreateCalendarEvent) // "/api/calendar-events"
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<CalendarEventCreateDto>>()
            .WithName("CreateCalendarEvent");

        // Update calendar event.
        app.MapPut("/api/calendar-event/update", UpdateCalendarEvent) // "/api/calendar-events/{calendarEventId:int}"
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<CalendarEventUpdateDto>>()
            .WithName("UpdateCalendarEvent");

        // Delete calendar event.
        app.MapDelete("/api/calendar-event/delete/{calendarEventId:int}", DeleteCalendarEvent) // "/api/calendar-events/{calendarEventId:int}"
            .RequireAuthorization()
            .WithName("DeleteCalendarEvent");
        
        // Get all calendar events.
        app.MapGet("/api/calendar-events", GetCalendarEvents) // "/api/calendar-events"
            .RequireAuthorization()
            .WithName("GetCalendarEvents");

        // #pragma warning restore ASP0022
    }
    
    private static async Task<IResult> GetCalendarEventsByStableId(
        ICalendarEventService calendarEventService,
        int stableId)
    {
        return Result.Generate(await calendarEventService.GetCalendarEventsByStableIdAsync(stableId));
    }
    
    private static async Task<IResult> GetCalendarEvent(
        ICalendarEventService calendarEventService,
        int calendarEventId)
    {
        return Result.Generate(await calendarEventService.GetCalendarEventAsync(calendarEventId));
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
        int calendarEventId)
    {
        return Result.Generate(await calendarEventService.DeleteCalendarEventAsync(calendarEventId));
    }
    
    private static async Task<IResult> GetCalendarEvents(
        ICalendarEventService calendarEventService)
    {
        return Result.Generate(await calendarEventService.GetCalendarEventsAsync());
    }
}