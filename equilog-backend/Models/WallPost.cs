using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models
{
    public class WallPost
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string? Title { get; set; }

        [StringLength(280)]
        public string? Body { get; set; }

        public DateTime? PostDate { get; set; }

        public DateTime? LastEdited { get; set; }

        [ForeignKey("Stable")]
        public required int StableIdFk { get; set; }
        public virtual Stable? Stable { get; set; }
    }
}
