namespace equilog_backend.DTOs.StableDTOs;

public class StableCreateDto
{
    public required string Name { get; init; }

    public required string Type { get; init; }

    public required string County { get; init; }

    public string? Address { get; init; }

    public string? PostCode { get; init; }

    public int? BoxCount { get; init; }
}