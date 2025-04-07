using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.UserDTOs
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }

        [Required]
        [StringLength(254)]
        public required string Email { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }
    }
}
