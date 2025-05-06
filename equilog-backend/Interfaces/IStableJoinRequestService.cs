using equilog_backend.Common;
using equilog_backend.DTOs.StableJoinRequestDTOs;
using equilog_backend.DTOs.UserDTOs;

namespace equilog_backend.Interfaces;

public interface IStableJoinRequestService
{
    Task<ApiResponse<List<UserDto>?>> GetStableJoinRequestsByStableAsync(int stableId);
    
    Task<ApiResponse<Unit>> CreateStableJoinRequestAsync(StableJoinRequestDto stableJoinRequestDto);

    Task<ApiResponse<Unit>> AcceptStableJoinRequestAsync(StableJoinRequestDto stableJoinRequestDto);

    Task<ApiResponse<Unit>> DenyStableJoinRequestAsync(StableJoinRequestDto stableJoinRequestDto);
}