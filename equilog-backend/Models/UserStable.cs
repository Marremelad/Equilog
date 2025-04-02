using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class UserStable
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserIdFk { get; set; }
    public virtual required User User { get; set; }

    [ForeignKey("Stable")]
    public int StableIdFk { get; set; }
    public virtual required Stable Stable { get; set; } 
}