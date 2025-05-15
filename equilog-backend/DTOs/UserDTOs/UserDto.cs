namespace equilog_backend.DTOs.UserDTOs

{
    public class UserDto
    {
        public required int Id { get; init; }

        public required string FirstName { get; init; }

        public required string LastName { get; init; }
        
        public required string Email { get; init; }
        
        public string? PhoneNumber { get; init; }

        public string? EmergencyContact { get; init; }

        public string? CoreInformation { get; init; }

        public string? Description { get; init; }
    }
}
