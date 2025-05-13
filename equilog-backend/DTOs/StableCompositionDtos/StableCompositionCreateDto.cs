namespace equilog_backend.DTOs.StableCompositionDtos;

public class StableCompositionCreateDto
{
    public required int UserId { get; set; }

    public required string Name { get; set; }

    public required string Type { get; set; }

    public required string Address { get; set; }

    public required string County { get; set; }

    public required string PostCode { get; set; }

    public required int BoxCount { get; set; }
}