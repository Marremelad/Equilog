using equilog_backend.DTOs.UserDTOs;

namespace equilog_backend.DTOs.UserHorseDTOs
{
    public class UserWithUserHorseRoleDto
    {
		public required UserDto User { get; set; }

		public required int UserRole { get; set; }
    }
}
