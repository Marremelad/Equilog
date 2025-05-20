using equilog_backend.Common;
using equilog_backend.DTOs.BlobStorageDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Endpoints;

public class BlobStorageEndpoints
{
    public static void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/api/blob-storage/get-upload-uri", GetUploadUri)
            .WithName("GetUploadUri");

        app.MapGet("/api/blob-storage/get-read-uri", GetReadUri)
            .WithName("GetReadUri");
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
}