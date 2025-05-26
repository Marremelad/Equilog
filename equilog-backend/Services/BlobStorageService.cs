using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using equilog_backend.Common;
using equilog_backend.Interfaces;
using System.Net;
using Azure.Storage.Blobs.Models;

namespace equilog_backend.Services;

// Service that handles Azure Blob Storage operations for managing media files.
// Provides secure access through SAS (Shared Access Signature) URIs with time-limited permissions.
public class BlobStorageService(BlobServiceClient client) : IBlobStorageService
{
    // Container name where all Equilog media files are stored.
    private const string ContainerName = "equilog-media";
    
    // Time duration for which generated SAS URIs remain valid.
    private static readonly TimeSpan Validity = TimeSpan.FromMinutes(5);

    // Azure blob container client for performing operations on the media container.
    private readonly BlobContainerClient _container = client.GetBlobContainerClient(ContainerName);

    // Generates a time-limited read-only URI for accessing a specific blob.
    public async Task<ApiResponse<Uri?>> GetReadUriAsync(string blobName)
    {
        try
        {
            // Calculate when the SAS URI should expire.
            var expiresOn = DateTimeOffset.UtcNow.Add(Validity);
            var blobClient = _container.GetBlobClient(blobName);
            
            // Check if the requested blob actually exists in storage.
            bool exists = await blobClient.ExistsAsync();
        
            // Returns an error if the blob doesn't exist.
            if (!exists)
                return ApiResponse<Uri?>.Failure(
                    HttpStatusCode.NotFound,
                    $"Blob {blobName} does not exist.");
                
            // Generate SAS URI with read-only permissions.
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

    // Generates multiple time-limited read-only URIs for accessing a collection of blobs.
    public async Task<ApiResponse<List<Uri>?>> GetReadUrisAsync(IEnumerable<string> blobNames)
    {
        try
        {
            // Calculate when all SAS URIs should expire.
            var expiresOn = DateTimeOffset.UtcNow.Add(Validity);
        
            // Create tasks to check existence and generate URIs for each blob concurrently.
            var tasks = blobNames.Select(async name => 
            {
                var blobClient = _container.GetBlobClient(name);
                bool exists = await blobClient.ExistsAsync();

                // Returns an error response if the blob doesn't exist.
                if (!exists)
                    return ApiResponse<Uri>.Failure(
                        HttpStatusCode.NotFound,
                        $"Blob {name} does not exist.");

                // Generate SAS URI with read-only permissions.
                var uri = blobClient.GenerateSasUri(BlobSasPermissions.Read, expiresOn);
                
                return ApiResponse<Uri>.Success(
                    HttpStatusCode.OK,
                    uri,
                    "Read uri fetched successfully.");
            });

            // Wait for all blob operations to complete.
            var results = await Task.WhenAll(tasks);
            
            // Check if any blob operations failed.
            var failures = results.Where(r => !r.IsSuccess).ToList();
            
            // If any blobs failed, return combined error messages.
            if (failures.Count != 0)
            {
                var errorMessages = failures.Select(f => f.Message).ToList();
                
                return ApiResponse<List<Uri>?>.Failure(
                    HttpStatusCode.NotFound,
                    string.Join("; ", errorMessages));
            }

            // Extract successful URIs from results.
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

    // Generates a time-limited upload URI for creating or overwriting a blob.
    public Task<ApiResponse<Uri?>> GetUploadUriAsync(string blobName)
    {
        try
        {
            // Ensure the container exists with public blob access.
            _container.CreateIfNotExists(PublicAccessType.Blob);
        
            // Calculate when the SAS URI should expire.
            var expiresOn = DateTimeOffset.UtcNow.Add(Validity);
            var blobClient = _container.GetBlobClient(blobName);
        
            // Generate SAS URI with create and write permissions for uploading.
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
        
    // Deletes a blob from storage if it exists.
    public async Task<ApiResponse<Unit>> DeleteBlobAsync(string blobName)
    {
        try
        {
            var blobClient = _container.GetBlobClient(blobName);
            
            // Delete the blob only if it exists (won't throw an error if it doesn't exist).
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