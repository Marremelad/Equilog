using System.Net;
using equilog_backend.Common;
using equilog_backend.Interfaces;

namespace equilog_backend.Compositions;

public class BlobStorageComposition(
    IBlobStorageService blobStorageService,
    IUserService userService) : IBlobStorageComposition
{
    public async Task<ApiResponse<Unit>> SetProfilePictureCompositionAsync(int userId, string blobName)
    {
        try
        {
            var blobResponse = await blobStorageService.GetReadUriAsync(blobName);
            
            if (!blobResponse.IsSuccess)
                return ApiResponse<Unit>.Failure(blobResponse.StatusCode,
                    blobResponse.Message);

            var uri = blobResponse.Value!.ToString();

            var userResponse = await userService.SetProfilePictureAsync(userId, uri);

            if (!userResponse.IsSuccess)
                return userResponse;
            
            return ApiResponse<Unit>.Success(HttpStatusCode.OK,
                Unit.Value,
                $"Profile picture for user '{userId}' was set successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse<Unit>.Failure(HttpStatusCode.InternalServerError,
                ex.Message);
        }
    }
}