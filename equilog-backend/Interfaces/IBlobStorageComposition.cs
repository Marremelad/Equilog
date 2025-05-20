using equilog_backend.Common;

namespace equilog_backend.Interfaces;

public interface IBlobStorageComposition
{
    Task<ApiResponse<Unit>> SetProfilePictureCompositionAsync(int userId, string blobName);
}