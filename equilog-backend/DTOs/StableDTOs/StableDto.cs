namespace equilog_backend.DTOs.StableDTOs;

public class StableDto
{
    public required int Id { get; init; }
    
    public required string Name { get; set; }
    
    public required string Type { get; set; }
    
    public required string County { get; set; }
    
    public required int MemberCount { get; set; }
    
    public required int HorseCount { get; set; }
    
    public required int BoxCount { get; set; }
}