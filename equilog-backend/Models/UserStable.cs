using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class UserStable
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public required int UserIdFk { get; set; }
    public virtual User? User { get; set; }

    [ForeignKey("Stable")]
    public required int StableIdFk { get; set; }
    public virtual Stable? Stable { get; set; }

    public required int Role { get; set; } // 0 = Master admin | 1 = User admin | 2 = user.
}