namespace equilog_backend.DTOs.StableDTOs;

public class StableSearchParametersDto
{
    public required string SearchTerm { get; set; }

    public required int Page { get; init; } = 0;

    public required int PageSize { get; init; } = 10;
}