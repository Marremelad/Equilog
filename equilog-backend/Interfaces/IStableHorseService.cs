using equilog_backend.Common;
using equilog_backend.DTOs.StableHorseDTOs;

namespace equilog_backend.Interfaces;

public interface IStableHorseService
{
    Task<ApiResponse<int>> CreateStableHorseConnectionAsync(int stableId, int horseId);

    Task<ApiResponse<Unit>> RemoveHorseFromStableAsync(int stableHorseId);

    Task<ApiResponse<List<StableHorseDto>?>> GetStableHorsesAsync(int stableId);

    Task<ApiResponse<List<StableHorseOwnersDto>?>> GetHorsesWithOwnersByStableAsync(int stableId);
}