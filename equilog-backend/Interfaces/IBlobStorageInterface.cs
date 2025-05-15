using equilog_backend.Common;
using equilog_backend.DTOs.BlobDTOs;

namespace equilog_backend.Interfaces
{
    public interface IBlobStorageService
    {
        Task<ApiResponse<List<BlobSasInfoDto>?>> GetUploadSasUrisAsync(List<string> blobNames);
        Task<ApiResponse<List<BlobSasInfoDto>?>> GetDownloadSasUrisAsync(List<string> blobNames);

        Task<ApiResponse<bool>> DeleteBlobAsync(string blobName);
        Task<ApiResponse<List<string>?>> ListBlobsAsync();
    }
}
