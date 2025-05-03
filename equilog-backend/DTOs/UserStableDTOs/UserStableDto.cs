namespace equilog_backend.DTOs.UserStableDTOs
{
    public class UserStableDto
    {
        public required int UserIdFk { get; set; }

        public required int StableIdFk { get; set; }

        public required int Role { get; set; }
    }
}
