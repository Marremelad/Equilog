using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.UserDTOs
{
    public class UserDto
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
