using System.Net;
using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.CalendarEventDTOs;
using equilog_backend.Interfaces;
using equilog_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace equilog_backend.Services;

// Service that manages calendar events for stables, including CRUD operations.
// Handles scheduling and management of events associated with specific stables and users.
public class CalendarEventService(EquilogDbContext context, IMapper mapper) : ICalendarEventService
{
    // Retrieves all calendar events associated with a specific stable.
    public async Task<ApiResponse<List<CalendarEventDto>?>> GetCalendarEventsByStableIdAsync(int stableId)
    {
        try
        {
            // Fetch calendar events for the stable and include user information.
            var calendarEventDtos = mapper.Map<List<CalendarEventDto>>(
                await context.CalendarEvents
                .Where(ce => ce.StableIdFk == stableId)
                .Include(ce => ce.User)
                .ToListAsync());

            // Provides an appropriate message based on whether events were found or not.
            var message = calendarEventDtos.Count == 0
                ? "Operation was successful but stable has no stored calendar events."
                : "Calendar events fetched successfully.";
            
            return ApiResponse<List<CalendarEventDto>>.Success(
                HttpStatusCode.OK,
                calendarEventDtos,
                message);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<CalendarEventDto>?>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Retrieves a specific calendar event by its ID.
    public async Task<ApiResponse<CalendarEventDto?>> GetCalendarEventAsync(int calendarEventId)
    {
        try
        {
            // Find the calendar event by ID.
            var calendarEvent = await context.CalendarEvents
                .Where(ce => ce.Id == calendarEventId)
                .FirstOrDefaultAsync();

            // Returns an error if the calendar event doesn't exist.
            if (calendarEvent == null) return ApiResponse<CalendarEventDto>.Failure(
                HttpStatusCode.NotFound,
                    "Error: Calendar event not found.");

            return ApiResponse<CalendarEventDto>.Success(
                HttpStatusCode.OK,
                mapper.Map<CalendarEventDto>(calendarEvent),
                "Calendar event fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<CalendarEventDto>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Creates a new calendar event in the system.
    public async Task<ApiResponse<CalendarEventDto?>> CreateCalendarEventAsync(CalendarEventCreateDto calendarEventCreateDto)
    {
        try
        {
            // Map the DTO to the entity model and add it to the database.
            var calendarEvent = mapper.Map<CalendarEvent>(calendarEventCreateDto);
            
            context.CalendarEvents.Add(calendarEvent);
            await context.SaveChangesAsync();

            return ApiResponse<CalendarEventDto>.Success(
                HttpStatusCode.Created,
                mapper.Map<CalendarEventDto>(calendarEvent),
                "New calendar event created successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<CalendarEventDto>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    // Updates an existing calendar event with new information.
    public async Task<ApiResponse<Unit>> UpdateCalendarEventAsync(CalendarEventUpdateDto calendarEventUpdateDto)
    {
        try
        {
            // Find the existing calendar event to update.
            var calendarEvent = await context.CalendarEvents
                .Where(ce => ce.Id == calendarEventUpdateDto.Id)
                .FirstOrDefaultAsync();

            // Returns an error if the calendar event doesn't exist.
            if (calendarEvent == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Calendar event not found.");

            // Apply updates from DTO to the existing entity and save changes.
            mapper.Map(calendarEventUpdateDto, calendarEvent);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "Calendar event updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    } 
    
    // Removes a calendar event from the system.
    public async Task<ApiResponse<Unit>> DeleteCalendarEventAsync(int calendarEventId)
    {
        try
        {
            // Find the calendar event to delete.
            var calendarEvent = await context.CalendarEvents
                .Where(ce => ce.Id == calendarEventId)
                .FirstOrDefaultAsync();

            // Returns an error if the calendar event doesn't exist.
            if (calendarEvent == null)
                return ApiResponse<Unit>.Failure(
                    HttpStatusCode.NotFound,
                    "Error: Calendar event not found.");

            // Remove the calendar event and save changes.
            context.CalendarEvents.Remove(calendarEvent);
            await context.SaveChangesAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                $"Calendar event with id '{calendarEventId}' was deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
    
    // Retrieves all calendar events in the system (used for testing purposes).
    public async Task<ApiResponse<List<CalendarEventDto>?>> GetCalendarEventsAsync()
    {
        try
        {
            // Fetch all calendar events without filtering.
            var calendarEventDtos = mapper.Map<List<CalendarEventDto>>(
                await context.CalendarEvents
                .ToListAsync());
    
            return ApiResponse<List<CalendarEventDto>>.Success(
                HttpStatusCode.OK,
                calendarEventDtos,
                "Calendar events fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<CalendarEventDto>>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}
