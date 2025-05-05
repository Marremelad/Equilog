namespace equilog_backend.DTOs.UserDTOs
{
    public class UserUpdateDto
    {
        public required int Id { get; init; }

        public required string FirstName { get; init; }

        public required string LastName { get; init; }
    }
}
