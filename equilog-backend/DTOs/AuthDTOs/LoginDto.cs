using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.AuthDTOs;

public class LoginDto
{
    [Required] 
    public required string UserName { get; set; }

    [Required]
    public required string Password { get; set; }
}