using equilog_backend.DTOs.HorseDTOs;

namespace equilog_backend.DTOs.HorseCompositionDTOs;

public class HorseCompositionCreateDto
{
    public required int UserId { get; init; }

    public required int StableId { get; set; }

    public required HorseCreateDto Horse { get; set; }
}