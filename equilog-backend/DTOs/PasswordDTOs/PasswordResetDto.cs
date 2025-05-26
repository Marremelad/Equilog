using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.PasswordDTOs;

public class PasswordResetDto
{
    public required string Token { get; init; }
    
    public required string NewPassword { get; init; }
    
    public required string ConfirmPassword { get; init; }
}