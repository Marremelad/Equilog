using System.ComponentModel.DataAnnotations;

namespace equilog_backend.Models;

public class Comment
{
    [Key]
    public int Id { get; set; }
    
    public required DateTime CommentDate { get; set; }
    
    [StringLength(4094)]
    public required string Content { get; set; }
    
    public virtual List<UserComment>? UserComments { get; set; }
    public virtual List<StablePostComment>? StablePostComments { get; set; }
}