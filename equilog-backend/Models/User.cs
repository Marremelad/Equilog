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

    [StringLength(30)]
    public required string UserName { get; set; }

    [StringLength(100)]
    public required string PasswordHash { get; set; }
    
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    public virtual List<UserStable>? UserStables { get; set; }

    public virtual List<UserHorse>? UserHorses { get; set; }

    public virtual List<UserCalendarEvent>? UserEvents { get; set; }

    public virtual List<StablePost>? StablePost { get; set; }
    
    public virtual List<RefreshToken>? RefreshTokens { get; set; }
    
    public virtual List<StableJoinRequest>? StableJoinRequests { get; set; }
    
    public virtual List<StableInvite>? StableInvites { get; set; }
    
    public virtual List<StablePostComment>? StablePostComments { get; set; }
}