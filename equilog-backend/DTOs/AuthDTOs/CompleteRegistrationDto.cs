using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.AuthDTOs
{
    public class CompleteRegistrationDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string TwilioCode { get; set; } = string.Empty;
    }
}
