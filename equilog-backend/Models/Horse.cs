using System.ComponentModel.DataAnnotations;

namespace equilog_backend.Models
{
    public class Horse
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public required string Name { get; set; }

        public DateOnly? Age { get; set; }
        
        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(50)]
        public string? Breed { get; set; }

        [StringLength(254)]
        public string? CoreInformation { get; set; }

        [StringLength(254)]
        public string? Description { get; set; }

        public int? Weight { get; set; }

        public int? Height { get; set; }

        public int? CurrentBox { get; set; } 

        public virtual List<UserHorse>? UserHorses { get; set; }
        
        public virtual List<StableHorse>? StableHorses { get; set; }
    }
}
