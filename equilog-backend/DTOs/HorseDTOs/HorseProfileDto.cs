using equilog_backend.DTOs.UserHorseDTOs;

namespace equilog_backend.DTOs.HorseDTOs
{
    public class HorseProfileDto
    {
        public required HorseDto Horse { get; init; }
        public List<UserWithUserHorseRoleDto>? UserHorseRoles { get; init; }
    }
}
