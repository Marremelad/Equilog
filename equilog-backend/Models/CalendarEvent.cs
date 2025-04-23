using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class CalendarEvent
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public required string Title { get; set; }

    public required DateTime StartDateTime { get; set; }

    public required DateTime EndDateTime { get; set; }

    [StringLength(50)]
    public required string Type { get; set; }

    [ForeignKey("User")]
    public required int UserIdFk { get; set; }
    public virtual User? User { get; set; }

    [ForeignKey("Stable")]
    public required int StableIdFk { get; set; }
    public virtual Stable? Stable { get; set; }
    
    public virtual List<UserCalendarEvent>? UserCalendarEvents { get; set; }
}