using equilog_backend.Common;
using equilog_backend.DTOs.StableDTOs;
using equilog_backend.DTOs.StableJoinRequestDTOs;
using equilog_backend.DTOs.UserDTOs;

namespace equilog_backend.Interfaces;

public interface IStableJoinRequestService
{
    Task<ApiResponse<List<UserDto>?>> GetStableJoinRequestsByStableIdAsync(int stableId);
    
    Task<ApiResponse<List<StableDto>?>> GetStableJoinRequestsByUserIdAsync(int userId);
    
    Task<ApiResponse<Unit>> CreateStableJoinRequestAsync(StableJoinRequestDto stableJoinRequestDto);

    Task<ApiResponse<Unit>> AcceptStableJoinRequestAsync(StableJoinRequestDto stableJoinRequestDto);

    Task<ApiResponse<Unit>> RefuseStableJoinRequestAsync(StableJoinRequestDto stableJoinRequestDto);
}