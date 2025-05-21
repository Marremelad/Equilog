namespace equilog_backend.DTOs.UserDTOs
{
    public class UserUpdateDto
    {
    public int Id { get; init; }
    
    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string Email { get; init; }

    public string? EmergencyContact { get; set; }

    public string? CoreInformation { get; set; }

    public string? Description { get; set; }
 
    public string? PhoneNumber { get; set; }
    }
}
