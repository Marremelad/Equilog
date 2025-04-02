using equilog_backend.Common.Enums;
using equilog_backend.Services;

namespace equilog_backend.Endpoints;

public class EventEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/events", async (EventService eventService) =>
        {
            var response = await eventService.GetAllEvents();

            return response.OperationResult switch
            {
                OperationResult.Success => Results.Ok(response.Value),
                _ => Results.Problem(response.Error, statusCode: 500)
            };
        });
    }
}