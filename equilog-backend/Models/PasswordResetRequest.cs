using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class PasswordResetRequest
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public required int UserIdFk { get; set; }
    public virtual User? User { get; set; }

    [StringLength(38)]
    public required string ResetCode { get; set; }

    public required DateTime ExpirationDate { get; set; }
}