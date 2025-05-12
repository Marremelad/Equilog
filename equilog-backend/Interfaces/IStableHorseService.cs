using equilog_backend.Common;

namespace equilog_backend.Interfaces;

public interface IStableHorseService
{
    Task<ApiResponse<Unit>> CreateStableHorseConnectionAsync(int stableId, int horseId);
}