using equilog_backend.Common;

namespace equilog_backend.Interfaces
{
    public interface IBlobService
    {
        Task<Uri> GetReadUriAsync(string blobName);
        Task<List<Uri>> GetReadUrisAsync(IEnumerable<string> blobNames);
        Task<Uri> GetUploadUriAsync(string blobName);
        Task<ApiResponse<Unit>> DeleteBlobAsync(string blobName);
    }
}
