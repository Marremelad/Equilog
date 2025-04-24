using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class PasswordResetRequest
{
    [Key]
    public int Id { get; set; }

    [EmailAddress]
    [StringLength(254)]
    public required string Email { get; set; }

    [StringLength(38)]
    public required string ResetCode { get; set; }

    public required DateTime ExpirationDate { get; set; }
}