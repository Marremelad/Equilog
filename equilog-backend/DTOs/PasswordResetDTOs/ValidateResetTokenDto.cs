namespace equilog_backend.DTOs.PasswordResetDTOs;

public class ValidateResetTokenDto
{
    public required string Email { get; set; }
    public required string Token { get; set; } 
}