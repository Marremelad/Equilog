using equilog_backend.DTOs.HorseDTOs;

namespace equilog_backend.DTOs.UserHorseDTOs
{
	public class HorseWithUserHorseRoleDto
	{
		public required HorseDto Horse { get; init; }

		public required int UserRole { get; init; }
	}
}
