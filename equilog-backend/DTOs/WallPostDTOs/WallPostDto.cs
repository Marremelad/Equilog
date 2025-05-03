using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.WallPostDTOs
{
    public class WallPostDto
    {
        [Required]
        public int StableIdFk { get; init; }

        public string? Title { get; init; }

        public string? Body { get; init; }

        public DateTime? PostDate { get; init; }

        public DateTime? LastEdited { get; init; }
    }
}
