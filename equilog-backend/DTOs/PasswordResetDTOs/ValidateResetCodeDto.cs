namespace equilog_backend.DTOs.PasswordResetDTOs;

public class ValidateResetCodeDto
{
    public required string Email { get; set; }
    public required string ResetCode { get; set; } 
}