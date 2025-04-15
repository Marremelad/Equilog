using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.AuthDTOs;

public class RegisterDto
{
    [Required]
    [StringLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public required string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public required string Email { get; set; }
    
    [Required]
    [StringLength(30)]
    public required string UserName { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public required string Password { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }
}