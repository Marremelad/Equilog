using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using equilog_backend.Common;
using equilog_backend.Interfaces;
using System.Net;
using Azure.Storage.Blobs.Models;

namespace equilog_backend.Services;

public class BlobStorageService(BlobServiceClient client) : IBlobStorageService
{
    private const string ContainerName = "equilog-media";
    private static readonly TimeSpan Validity = TimeSpan.FromMinutes(5);

    private readonly BlobContainerClient _container = client.GetBlobContainerClient(ContainerName);

    public async Task<ApiResponse<Uri?>> GetReadUriAsync(string blobName)
    {
        try
        {
            var expiresOn = DateTimeOffset.UtcNow.Add(Validity);
            var blobClient = _container.GetBlobClient(blobName);
            bool exists = await blobClient.ExistsAsync();
        
            if (!exists)
                return ApiResponse<Uri?>.Failure(
                    HttpStatusCode.NotFound,
                    $"Blob {blobName} does not exist.");
                
            var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, expiresOn);
            
            return ApiResponse<Uri?>.Success(
                HttpStatusCode.OK,
                sasUri,
                "Sas uri fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Uri?>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
        
    }

    public async Task<ApiResponse<List<Uri>?>> GetReadUrisAsync(IEnumerable<string> blobNames)
    {
        try
        {
            var expiresOn = DateTimeOffset.UtcNow.Add(Validity);
        
            var tasks = blobNames.Select(async name => 
            {
                var blobClient = _container.GetBlobClient(name);
                bool exists = await blobClient.ExistsAsync();

                if (!exists)
                    return ApiResponse<Uri>.Failure(
                        HttpStatusCode.NotFound,
                        $"Blob {name} does not exist.");

                var uri = blobClient.GenerateSasUri(BlobSasPermissions.Read, expiresOn);
                
                return ApiResponse<Uri>.Success(
                    HttpStatusCode.OK,
                    uri,
                    "Read uri fetched successfully.");
            });

            var results = await Task.WhenAll(tasks);
            var failures = results.Where(r => !r.IsSuccess).ToList();
            
            if (failures.Count != 0)
            {
                var errorMessages = failures.Select(f => f.Message).ToList();
                
                return ApiResponse<List<Uri>?>.Failure(
                    HttpStatusCode.NotFound,
                    string.Join("; ", errorMessages));
            }

            var uris = results.Where(r => r.IsSuccess).Select(r => r.Value!).ToList();
            
            return ApiResponse<List<Uri>?>.Success(
                HttpStatusCode.OK,
                uris, 
                "Read uris fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<List<Uri>?>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public Task<ApiResponse<Uri?>> GetUploadUriAsync(string blobName)
    {
        try
        {
            _container.CreateIfNotExists(PublicAccessType.Blob);
        
            var expiresOn = DateTimeOffset.UtcNow.Add(Validity);
            var blobClient = _container.GetBlobClient(blobName);
        
            var sasUri = blobClient.GenerateSasUri(
                BlobSasPermissions.Create | BlobSasPermissions.Write,
                expiresOn);
            
            return Task.FromResult(ApiResponse<Uri>.Success(
                HttpStatusCode.OK,
                sasUri,
                "Upload uri generated successfully.")); 
        }
        catch (Exception ex)
        {
            return Task.FromResult(ApiResponse<Uri>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message));
        }
    }
        
    public async Task<ApiResponse<Unit>> DeleteBlobAsync(string blobName)
    {
        try
        {
            var blobClient = _container.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();

            return ApiResponse<Unit>.Success(
                HttpStatusCode.OK,
                Unit.Value,
                "Blob deleted successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}