using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class StablePostComment
{
    [Key]
    public int Id { get; set; }
    
    public required DateTime CommentDate { get; set; }
    
    [StringLength(4094)]
    public required string Content { get; set; }
    
    [ForeignKey("User")]
    public required int UserIdFk { get; set; }
    public virtual User? User { get; set; }
    
    [ForeignKey("StablePost")]
    public required int StablePostIdFk { get; set; }
    public virtual StablePost? StablePost { get; set; }
}