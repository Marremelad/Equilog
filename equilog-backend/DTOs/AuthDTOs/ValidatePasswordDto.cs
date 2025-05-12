namespace equilog_backend.DTOs.AuthDTOs;

public class ValidatePasswordDto
{
    public required int UserId { get; init; }
    
    public required string Password { get; init; }
}