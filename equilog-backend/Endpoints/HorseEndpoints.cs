using equilog_backend.Common.Enums;
using equilog_backend.Services;

namespace equilog_backend.Endpoints;

public class HorseEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/horses", async (HorseService horseService) =>
        {
            var response = await horseService.GetAllHorses();
            
            return response.OperationResult switch
            {
                OperationResult.Success => Results.Ok(response.Value),
                _ => Results.Problem(response.Error, statusCode: 500)
            };
        });
    }
}