using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.HorseDTOs
{
    public class HorseDto
    {
        [Required]
        public required int Id { get; set; }
        
        [Required]
        public required string Name { get; set; }
        
        public DateTime? Age { get; set; }
        
        public string? Color { get; set; }
        
        public string? Breed { get; set; }
    }
}
