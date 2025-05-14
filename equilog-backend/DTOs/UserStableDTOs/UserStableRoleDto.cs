using equilog_backend.Models;

namespace equilog_backend.DTOs.UserStableDTOs
{
    public class UserStableRoleDto
    {
        public required User User { get; set; }

        public required int Role { get; set; }
    }
}
