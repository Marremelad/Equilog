using equilog_backend.Common;
using equilog_backend.CompositionInterfaces;
using equilog_backend.DTOs.StableDTOs;

namespace equilog_backend.CompositionEndpoints;

public class StableCompositionEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Create stable with wall post.
        app.MapPost("/api/stable/create-with-wall-post", CreateStableWithWallPost)
            .AddEndpointFilter<ValidationFilter<StableCreateDto>>()
            .WithName("CreateStableWithWallPost");
    }

    private static async Task<IResult> CreateStableWithWallPost(
        IStableCompositionService stableCompositionService, 
        StableCreateDto stableCreateDto)
    {
        var result = await stableCompositionService.CreateStableWithWallPostAsync(stableCreateDto);
        return Result.Generate(result);
    }
}