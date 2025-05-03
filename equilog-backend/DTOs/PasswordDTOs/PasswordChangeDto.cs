using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.PasswordDTOs;

public class PasswordChangeDto
{
    [EmailAddress]
    public required string Email { get; set; }
    
    [StringLength(100, MinimumLength = 8)]
    public required string NewPassword { get; set; }
    
    [StringLength(100, MinimumLength = 8)]
    public required string ConfirmPassword { get; set; }
}