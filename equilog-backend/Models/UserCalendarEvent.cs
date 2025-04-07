using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class UserCalendarEvent
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserIdFk { get; set; }
    public virtual required User User { get; set; }

    [ForeignKey("Event")]
    public required int EventIdFk { get; set; }
    public virtual CalendarEvent? CalendarEvent { get; set; }
}