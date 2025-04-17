using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.WallPostDTOs
{
    public class WallPostClearDto
    {
        public string? Title { get; set; } = null;

        public string? Body { get; set; } = null;

        public DateTime? PostDate { get; set; } = null;

        public DateTime? LastEdited { get; set; } = null;
    }
}
