using equilog_backend.Common;
using equilog_backend.DTOs.StableInviteDTOs;

namespace equilog_backend.Interfaces;

public interface IStableInviteService
{
    Task<ApiResponse<Unit>> CreateStableInviteAsync(StableInviteDto stableInviteDto);
}