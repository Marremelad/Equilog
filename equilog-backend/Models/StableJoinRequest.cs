using System.ComponentModel.DataAnnotations;

namespace equilog_backend.Models;

public class StableJoinRequest
{
    [Key]
    public int Id { get; set; }

    public required int UserIdFk { get; set; }

    public required int StableIdFk { get; set; }

    public required bool Accepted { get; set; }
}