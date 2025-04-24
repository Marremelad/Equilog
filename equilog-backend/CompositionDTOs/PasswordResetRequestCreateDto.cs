using System.ComponentModel.DataAnnotations;

namespace equilog_backend.CompositionDTOs;

public class PasswordResetRequestCreateDto
{
    [EmailAddress]
    public required string Email { get; set; }
}