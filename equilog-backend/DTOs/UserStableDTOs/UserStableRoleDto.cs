using equilog_backend.DTOs.UserDTOs;

namespace equilog_backend.DTOs.UserStableDTOs
{
    public class UserStableRoleDto
    {
        public required UserDto User { get; init; }

        public required int Role { get; init; }
    }
}
