namespace equilog_backend.DTOs.StableDTOs;

public class StableCreateDto
{
    public required string Name { get; init; }
    
    public required string Type { get; init; }
    
    public required string Address { get; init; }
    
    public required string County { get; init; }

    public required int PostCode { get; init; }

    public required int BoxCount { get; init; }
}