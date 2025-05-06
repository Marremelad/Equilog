using equilog_backend.Common;

namespace equilog_backend.Interfaces;

public interface IStableJoinRequestService
{
    Task<Unit> CreateStableJoinRequest();
}