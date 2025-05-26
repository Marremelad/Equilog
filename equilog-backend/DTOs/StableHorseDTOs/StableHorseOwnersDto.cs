namespace equilog_backend.DTOs.StableHorseDTOs
{
    public class StableHorseOwnersDto
    {
        public required int HorseId { get; init; }

        public required string HorseName { get; init; }

        public string? HorseColor { get; init; }
        
        public string? HorseBreed { get; init; }

        public List<string> HorseOwners { get; init; } = [];
    }
}
