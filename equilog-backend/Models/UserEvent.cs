using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class UserEvent
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserIdFk { get; set; }
    public virtual required User User { get; set; }

    [ForeignKey("Event")]
    public int EventIdFk { get; set; }
    public virtual required Event Event { get; set; }
}