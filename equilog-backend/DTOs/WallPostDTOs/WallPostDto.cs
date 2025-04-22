using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.WallPostDTOs
{
    public class WallPostDto
    {
        [Required]
        public int StableIdFk { get; set; }

        public string? Title { get; set; } = null;

        public string? Body { get; set; } = null;

        public DateTime? PostDate { get; set; } = null;

        public DateTime? LastEdited { get; set; } = null;
    }
}
