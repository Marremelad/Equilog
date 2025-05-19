using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using equilog_backend.Common;
using equilog_backend.Interfaces;
using System.Net;

namespace equilog_backend.Services
{
    public class BlobService : IBlobService
    {
        private const string ContainerName = "equilog-media";
        private static readonly TimeSpan Validity = TimeSpan.FromMinutes(5);

        private readonly BlobContainerClient _container;

        public BlobService(BlobServiceClient client)
        {
            _container = client.GetBlobContainerClient(ContainerName);
        }

        public async Task<Uri> GetReadUriAsync(string blobName)
        {
            var uris = await GetReadUrisAsync(new[] { blobName });
            return uris[0];
        }

        public async Task<List<Uri>> GetReadUrisAsync(IEnumerable<string> blobNames)
        {
            var expiresOn = DateTimeOffset.UtcNow.Add(Validity);
            var uris = new List<Uri>();

            foreach (var name in blobNames)
            {
                var blobClient = _container.GetBlobClient(name);
                uris.Add(blobClient.GenerateSasUri(BlobSasPermissions.Read, expiresOn));
            }

            return uris;
        }

        public Task<Uri> GetUploadUriAsync(string blobName)
        {
            var expiresOn = DateTimeOffset.UtcNow.Add(Validity);
            var blobClient = _container.GetBlobClient(blobName);
            var sas = blobClient.GenerateSasUri(
                BlobSasPermissions.Create | BlobSasPermissions.Write,
                expiresOn);
            return Task.FromResult(sas);
        }

        public async Task<ApiResponse<Unit>> DeleteBlobAsync(string blobName)
        {
            var blobClient = _container.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();

            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                   Unit.Value,
                   "Blob deleted successfully");
        }
    }
}
