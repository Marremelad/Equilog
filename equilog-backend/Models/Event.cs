using System.ComponentModel.DataAnnotations;

namespace equilog_backend.Models;

public class Event
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public required string Title { get; set; }

    public required DateTime StartDateTime { get; set; }

    public required DateTime EndDateTime { get; set; }
}