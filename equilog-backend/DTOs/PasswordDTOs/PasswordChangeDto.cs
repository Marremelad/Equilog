using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.PasswordResetDTOs;

public class PasswordChangeDto
{
    public required string Email { get; set; }

    public required string NewPassword { get; set; }

    public required string ConfirmPassword { get; set; }
}