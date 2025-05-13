using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class UserComment
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("User")]
    public required int UserIdFk { get; set; }
    public virtual User? User { get; set; }
    
    [ForeignKey("Comment")]
    public required int CommentIdFk { get; set; }
    public virtual Comment? Comment { get; set; }
}