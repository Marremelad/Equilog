using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.UserDTOs
{
    public class UserUpdateDto
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required string Name { get; set; }
    }
}
