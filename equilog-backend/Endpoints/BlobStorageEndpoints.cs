using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class BlobStorageEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        // Get upload uri.
        app.MapGet("/api/blob-storage/get-upload-uri", GetUploadUri) // "/api/blobs/{blobName}/upload-uri"
            .WithName("GetUploadUri");

        // Get read uri.
        app.MapGet("/api/blob-storage/get-read-uri", GetReadUri) // "/api/blobs/{blobName}/read-uri"
            .WithName("GetReadUri");

        // Delete blob from blob storage.
        app.MapDelete("/api/blob-storage/delete-blob", DeleteBlob) // "/api/blobs/{blobName}"
            .WithName("DeleteBlob");
    }

    private static async Task<IResult> GetUploadUri(
        IBlobStorageService blobStorageService,
        string blobName)
    {
        return Result.Generate(await blobStorageService.GetUploadUriAsync(blobName));
    }

    private static async Task<IResult> GetReadUri(
        IBlobStorageService blobStorageService,
        string blobName)
    {
        return Result.Generate(await blobStorageService.GetReadUriAsync(blobName));
    }

    private static async Task<IResult> DeleteBlob(
        IBlobStorageService blobStorageService,
        string blobName)
    {
        return Result.Generate(await blobStorageService.DeleteBlobAsync(blobName));
    }
}