using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class UserHorse
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserIdFk { get; set; }
    public virtual required User User { get; set; }
    
    [ForeignKey("Horse")]
    public int HorseIdFk { get; set; }
    public virtual required Horse Horse { get; set; }

    public required string UserRole { get; set; } // Change to enum.
}