using equilog_backend.Common;
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
        return Result.Generate(await calendarEventService.GetCalendarEvents());
    }

    private static async Task<IResult> GetCalendarEvent(ICalendarEventService calendarEventService, int id)
    {
        return Result.Generate(await calendarEventService.GetCalendarEvent(id));
    }
    
    private static async Task<IResult> CreateCalendarEvent(ICalendarEventService calendarEventService,
        CalendarEventCreateDto newCalendarEvent)
    {
        return Result.Generate(await calendarEventService.CreateCalendarEvent(newCalendarEvent));
    }

    private static async Task<IResult> UpdateCalendarEvent(ICalendarEventService calendarEventService,
        CalendarEventUpdateDto updatedEvent)
    {
        return Result.Generate(await calendarEventService.UpdateCalendarEvent(updatedEvent));
    }

    private static async Task<IResult> DeleteCalendarEvent(ICalendarEventService calendarEventService, int id)
    {
        return Result.Generate(await calendarEventService.DeleteCalendarEvent(id));
    }
}