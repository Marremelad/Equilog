using System.ComponentModel.DataAnnotations;

namespace equilog_backend.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public required string FirstName { get; set; }

    [StringLength(50)]
    public required string LastName { get; set; }
    
    [StringLength(254)]
    public required string Email { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    
    public virtual List<UserHorse>? UserHorses { get; set; }
}