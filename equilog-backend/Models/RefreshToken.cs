using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(128)]
    public required string Token { get; set; }
    
    [Required]
    public DateTime CreatedDate { get; set; }
    
    [Required]
    public DateTime ExpirationDate { get; set; }
    
    [Required]
    public bool IsRevoked { get; set; }
    
    [Required]
    public bool IsUsed { get; set; }
    
    [ForeignKey("User")]
    public required int UserIdFk { get; set; }
    public virtual User? User { get; set; }
}