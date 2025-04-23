using equilog_backend.DTOs.StableDTOs;
using equilog_backend.DTOs.WallPostDTOs;

namespace equilog_backend.DTOs.CompositionDTOs;

public class StableWithWallPostDto
{
    public required StableDto? Stable { get; set; }
    public required WallPostDto? WallPost { get; set; }
}