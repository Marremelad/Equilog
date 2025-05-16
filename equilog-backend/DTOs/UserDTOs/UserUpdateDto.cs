namespace equilog_backend.DTOs.UserDTOs
{
    public class UserUpdateDto
    {
    public int Id { get; set; }
    
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }

    public string? EmergencyContact { get; set; }

    public string? CoreInformation { get; set; }

    public string? Description { get; set; }
 
    public string? PhoneNumber { get; set; }
    }
}
