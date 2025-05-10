namespace equilog_backend.DTOs.UserStableDTOs
{
    public class UserStableDto
    {
        public required int UserIdFk { get; init; }

        public required int StableIdFk { get; init; }

        public required int Role { get; init; }
    }
}
