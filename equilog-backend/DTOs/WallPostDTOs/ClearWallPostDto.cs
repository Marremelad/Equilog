using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.WallPostDTOs
{
    public class ClearWallPostDto
    {
        [Required]
        public required int StableIdFk { get; set; }
    }
}
