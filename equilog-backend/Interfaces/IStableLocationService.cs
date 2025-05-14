using equilog_backend.Common;
using equilog_backend.DTOs.StableLocationDtos;

namespace equilog_backend.Interfaces
{
    public interface IStableLocationService
    {
        Task<ApiResponse<StableLocationDto?>> GetStableLocationAsync(string postnummer);
    }
}
