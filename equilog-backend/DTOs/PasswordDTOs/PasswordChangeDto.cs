namespace equilog_backend.DTOs.PasswordDTOs;

public class PasswordChangeDto
{
    public required string Email { get; set; }

    public required string NewPassword { get; set; }

    public required string ConfirmPassword { get; set; }
}