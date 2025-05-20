using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class BlobStorageEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/api/blob-storage/set-profile-picture", SetProfilePictureComposition)
            .WithName("SetProfilePicture");
    }

    private static async Task<IResult> GetUploadUri(
        IBlobStorageService blobStorageService,
        string blobName)
    {
        return Result.Generate(await blobStorageService.GetUploadUriAsync(blobName));
    }

    private static async Task<IResult> SetProfilePictureComposition(
        IBlobStorageComposition blobStorageComposition,
        int userId,
        string blobName)
    {
        return Result.Generate(await blobStorageComposition.SetProfilePictureCompositionAsync(userId, blobName));
    }
}