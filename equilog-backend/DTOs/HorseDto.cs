using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs
{
    public class HorseDTO
    {
        // for mapping/output ONLY
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Age { get; set; }
        public string? Color { get; set; }
        public string? Breed { get; set; }
    }
}
