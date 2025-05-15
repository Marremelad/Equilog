using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models
{
    public class StablePost
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public required int UserIdFk { get; set; }
        public virtual User? User { get; set; }

        [ForeignKey("Stable")]
        public required int StableIdFk { get; set; }
        public virtual Stable? Stable { get; set; }

        [StringLength(510)]
        public required string Title { get; set; }

        [StringLength(4094)]
        public required string Content { get; set; }

        public required DateTime Date { get; set; }

        public required bool IsPinned { get; set; }
        
        public virtual List<StablePostComment>? StablePostComments { get; set; }
    }
}
