using equilog_backend.Common;
using equilog_backend.DTOs.StableInviteDTOs;
using equilog_backend.DTOs.UserDTOs;

namespace equilog_backend.Interfaces;

public interface IStableInviteService
{
    Task<ApiResponse<Unit>> CreateStableInviteAsync(StableInviteDto stableInviteDto);

    Task<ApiResponse<Unit>> AcceptStableInviteAsync(StableInviteDto stableInviteDto);

    Task<ApiResponse<Unit>> RefuseStableInviteAsync(StableInviteDto stableInviteDto);

    Task<ApiResponse<List<UserDto>?>> GetStableInvitesByStableIdAsync(int stableId);
}