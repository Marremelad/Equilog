using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models
{
    public class StablePost
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserIdFk { get; set; }
        public virtual required User User { get; set; }

        [ForeignKey("Stable")]
        public int StableIdFk { get; set; }
        public virtual required Stable Stable { get; set; }

        [StringLength(510)]
        public required string Title { get; set; }

        [StringLength(4094)]
        public required string Content { get; set; }

        public required DateTime Date { get; set; }

        public required bool Pinned { get; set; }
    }
}
