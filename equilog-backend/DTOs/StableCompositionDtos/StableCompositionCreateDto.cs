using equilog_backend.DTOs.StableDTOs;

namespace equilog_backend.DTOs.StableCompositionDtos;

public class StableCompositionCreateDto
{
    public required int UserId { get; set; }

    public required StableCreateDto Stable { get; set; }
}