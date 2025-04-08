using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class StableHorse
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Stable")]
    public int StableIdFk { get; set; }
    public virtual required Stable Stable { get; set; }
    
    [ForeignKey("Horse")]
    public required int HorseIdFk { get; set; }
    public virtual Horse? Horse { get; set; }
}