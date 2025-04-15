using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models
{
    public class WallPost
    {
        [Key]
        [ForeignKey("Stable")]
        public int StableIdFk { get; set; }

        [StringLength(50)]
        public string? Title { get; set; }

        [StringLength(280)]
        public string? Body { get; set; }

        public DateTime? PostDate { get; set; }

        public DateTime? LastEdited { get; set; }

        public virtual Stable? Stable { get; set; }
    }
}
