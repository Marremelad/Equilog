using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class UserHorse
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public required int UserIdFk { get; set; }
    public virtual User? User { get; set; }
    
    [ForeignKey("Horse")]
    public required int HorseIdFk { get; set; }
    public virtual Horse? Horse { get; set; }

    [StringLength(20)]
    public required int UserRole { get; set; }
}