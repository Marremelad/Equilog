namespace equilog_backend.DTOs.UserDTOs
{
    public class UserUpdateDto
    {
        public required int Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }
    }
}
