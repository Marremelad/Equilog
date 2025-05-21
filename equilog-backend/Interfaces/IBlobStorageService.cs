using equilog_backend.Common;

namespace equilog_backend.Interfaces
{
    public interface IBlobStorageService
    {
        Task<ApiResponse<Uri?>> GetReadUriAsync(string blobName);

        Task<ApiResponse<List<Uri>?>> GetReadUrisAsync(IEnumerable<string> blobNames);

        Task<ApiResponse<Uri?>> GetUploadUriAsync(string blobName);
        
        Task<ApiResponse<Unit>> DeleteBlobAsync(string blobName);
    }
}
