using equilog_backend.DTOs.StableDTOs;
using equilog_backend.DTOs.WallPostDTOs;

namespace equilog_backend.CompositionDTOs;

public class StableWithWallPostDto
{
    public required StableDto? StableDto { get; set; }
    public required WallPostDto? WallPostDto { get; set; }
}