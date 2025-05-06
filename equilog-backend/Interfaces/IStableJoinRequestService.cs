using equilog_backend.Common;
using equilog_backend.DTOs.StableJoinRequestDTOs;

namespace equilog_backend.Interfaces;

public interface IStableJoinRequestService
{
    Task<ApiResponse<Unit>> CreateStableJoinRequest(StableJoinRequestDto stableJoinRequestDto);
}