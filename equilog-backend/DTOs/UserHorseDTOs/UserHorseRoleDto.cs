using equilog_backend.DTOs.HorseDTOs;

namespace equilog_backend.DTOs.UserHorseDTOs
{
	public class UserHorseRoleDto
	{
		public required HorseDto Horse { get; set; }

		public required int UserRole { get; set; }
	}
}
