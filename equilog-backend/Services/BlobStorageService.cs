using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using equilog_backend.Common;
using equilog_backend.Interfaces;
using System.Net;

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
                throw new FileNotFoundException($"Blob {blobName} does not exist.");
    
            var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, expiresOn);
            
            return ApiResponse<Uri?>.Success(HttpStatusCode.OK,
                sasUri,
                "Sas uri fetched successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Uri?>.Failure(HttpStatusCode.InternalServerError,
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
                    throw new FileNotFoundException($"Blob {name} does not exist.");
        
                return blobClient.GenerateSasUri(BlobSasPermissions.Read, expiresOn);
            });

            var uris = await Task.WhenAll(tasks);
            return ApiResponse<List<Uri>>.Success(
                HttpStatusCode.OK,
                uris.ToList(),
                null);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<Uri>>.Failure(
                HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }

    public Task<ApiResponse<Uri?>> GetUploadUriAsync(string blobName)
    {
        try
        {
            var expiresOn = DateTimeOffset.UtcNow.Add(Validity);
            var blobClient = _container.GetBlobClient(blobName);
            
            var sasUri = blobClient.GenerateSasUri(
                BlobSasPermissions.Create | BlobSasPermissions.Write,
                expiresOn);
        
            return Task.FromResult(ApiResponse<Uri>.Success(HttpStatusCode.OK,
                sasUri,
                null)); 
        }
        catch (Exception ex)
        {
            return Task.FromResult(ApiResponse<Uri>.Failure(HttpStatusCode.InternalServerError,
                ex.Message));
        }
    }
        
    public async Task<ApiResponse<Unit>> DeleteBlobAsync(string blobName)
    {
        try
        {
            var blobClient = _container.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();

            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                "Blob deleted successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}