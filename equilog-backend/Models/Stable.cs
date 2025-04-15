using System.ComponentModel.DataAnnotations;

namespace equilog_backend.Models;

public class Stable
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public required string Name { get; set; }

    public virtual List<UserStable>? UserStables { get; set; }

    public virtual List<StableHorse>? StableHorses { get; set; }

    public virtual List<StablePost>? StablePost { get; set; }

    public virtual WallPost? WallPost { get; set; }
}