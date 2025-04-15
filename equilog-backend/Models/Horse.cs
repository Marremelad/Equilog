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
        
        public virtual List<UserHorse>? UserHorses { get; set; }
        
        public virtual List<StableHorse>? StableHorses { get; set; }
    }
}
