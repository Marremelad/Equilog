using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.PasswordResetDTOs;

public class PasswordResetWithTokenDto
{
    [Required]
    public required string Token { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public required string NewPassword { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public required string ConfirmPassword { get; set; }
}