using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.WallPostDTOs
{
    public class WallPostUpdateDto
    {
        [Required]
        public required int StableIdFk { get; set; }

        [StringLength(40)]
        public string? Title { get; set; }

        [StringLength(280)]
        public string? Body { get; set; }
    }
}
