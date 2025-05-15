using System.Net;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using equilog_backend.Common;
using equilog_backend.DTOs.BlobDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Services
{
    public class BlobStorageService(BlobServiceClient blobServiceClient) : IBlobStorageService
    {
        private const string ContainerName = "equilog-media";
        private static readonly TimeSpan SasValidity = TimeSpan.FromMinutes(5);

        private readonly BlobContainerClient _containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

        private bool _containerInitialized;

        // Ensures the blob container exists, creating it if necessary.
        private async Task EnsureContainerExistsAsync()
        {
            if (!_containerInitialized)
            {
                await _containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
                _containerInitialized = true;
            }
        }

        // Generates SAS URIs for the specified blob names with the given permissions.
        private async Task<ApiResponse<List<BlobSasInfoDto>?>> GenerateSasUrisAsync(
            List<string> blobNames,
            BlobSasPermissions permissions,
            string successMessage)
        {
            var result = new List<BlobSasInfoDto>();

            try
            {
                await EnsureContainerExistsAsync();

                foreach (var name in blobNames)
                {
                    var blobClient = _containerClient.GetBlobClient(name);
                    var sasUri = blobClient.GenerateSasUri(
                        permissions,
                        DateTimeOffset.UtcNow.Add(SasValidity));

                    result.Add(new BlobSasInfoDto
                    {
                        BlobName = name,
                        SasUri = sasUri
                    });
                }

                return ApiResponse<List<BlobSasInfoDto>>.Success(
                    HttpStatusCode.OK,
                    result,
                    successMessage);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<BlobSasInfoDto>>.Failure(
                    HttpStatusCode.InternalServerError,
                    $"Failed to generate SAS URLs: {ex.Message}");
            }
        }

        // Generates upload SAS URIs for the specified blob names.
        public Task<ApiResponse<List<BlobSasInfoDto>?>> GetUploadSasUrisAsync(List<string> blobNames) =>
            GenerateSasUrisAsync(
                blobNames,
                BlobSasPermissions.Write | BlobSasPermissions.Create,
                "Upload SAS URL(s) generated successfully.");

        // Generates download SAS URIs for the specified blob names.
        public Task<ApiResponse<List<BlobSasInfoDto>?>> GetDownloadSasUrisAsync(List<string> blobNames) =>
            GenerateSasUrisAsync(
                blobNames,
                BlobSasPermissions.Read,
                "Download SAS URL(s) generated successfully.");

        // Generates delete SAS URIs for the specified blob names.
        public async Task<ApiResponse<bool>> DeleteBlobAsync(string blobName)
        {
            try
            {
                await EnsureContainerExistsAsync();
                var deleted = (await _containerClient
                    .GetBlobClient(blobName)
                    .DeleteIfExistsAsync())
                    .Value;

                return ApiResponse<bool>.Success(
                    HttpStatusCode.OK,
                    deleted,
                    deleted ? "Blob deleted." : "Blob not found.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure(
                    HttpStatusCode.InternalServerError,
                    $"Failed to delete blob: {ex.Message}");
            }
        }

        // Lists all blobs in the container.
        public async Task<ApiResponse<List<string>?>> ListBlobsAsync()
        {
            try
            {
                await EnsureContainerExistsAsync();

                var names = new List<string>();
                await foreach (var item in _containerClient.GetBlobsAsync())
                {
                    names.Add(item.Name);
                }

                return ApiResponse<List<string>>.Success(
                    HttpStatusCode.OK,
                    names,
                    "Blobs retrieved successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<string>>.Failure(
                    HttpStatusCode.InternalServerError,
                    $"Failed to list blobs: {ex.Message}");
            }
        }
    }
}
