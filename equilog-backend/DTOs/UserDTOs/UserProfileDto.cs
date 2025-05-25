using equilog_backend.DTOs.UserHorseDTOs;
using equilog_backend.DTOs.UserStableDTOs;

namespace equilog_backend.DTOs.UserDTOs;

public class UserProfileDto
{
    public required UserStableRoleDto UserStableRole { get; init; }
    public List<HorseWithUserHorseRoleDto>? UserHorseRoles { get; init; }
}