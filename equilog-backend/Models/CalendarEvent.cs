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

    [ForeignKey("Stable")]
    public int StableIdFk { get; set; }
    public virtual required Stable Stable { get; set; }
    
    public virtual List<UserCalendarEvent>? UserCalendarEvents { get; set; }
}