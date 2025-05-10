namespace equilog_backend.DTOs.StableDTOs;

public class StableSearchDto
{
    public int Id { get; init; }
        
    public required string Name { get; init; }
        
    public string? Address { get; init; }
        
    public required string County { get; init; }
}