namespace equilog_backend.DTOs.StableDTOs;

public class StableDto
{
    public required int Id { get; init; }
    
    public required string Name { get; init; }
    
    public required string Type { get; init; }
    
    public required string Address { get; init; }
    
    public required string County { get; init; }
    
    public required int PostCode { get; init; }
    
    public required int MemberCount { get; init; }
    
    public required int HorseCount { get; init; }
    
    public required int BoxCount { get; init; }
}