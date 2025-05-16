using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models;

public class StablePostComment
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("StablePost")]
    public required int StablePostIdFk { get; set; }
    public virtual StablePost? StablePost { get; set; }
    
    [ForeignKey("Comment")]
    public required int CommentIdFk { get; set; }
    public virtual Comment? Comment { get; set; }
}