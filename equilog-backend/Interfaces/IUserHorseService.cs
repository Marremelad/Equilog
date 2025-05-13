using equilog_backend.Common;

namespace equilog_backend.Interfaces
{
    public interface IUserHorseService
    {
        Task<ApiResponse<Unit>> CreateUserHorseConnectionAsync(int userId, int horseId);
    }
}
