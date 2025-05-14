using equilog_backend.DTOs.StableHorseDTOs;
using equilog_backend.DTOs.UserHorseDTOs;
using equilog_backend.DTOs.UserStableDTOs;
using equilog_backend.Models;

namespace equilog_backend.DTOs.UserDTOs
{
    public class UserProfileDto
    {
        public required UserStableRoleDto UserStableRole { get; init; }
        public List<UserHorseRoleDto>? UserHorses { get; init; }
    }
}
