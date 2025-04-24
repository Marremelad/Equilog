using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.PasswordResetDTOs;

public class PasswordResetDto
{
    [EmailAddress]
    public required string Email { get; set; }
    
    [StringLength(100, MinimumLength = 8)]
    public required string NewPassword { get; set; }
    
    [StringLength(100, MinimumLength = 8)]
    public required string NewPasswordConfirmation { get; set; }
}