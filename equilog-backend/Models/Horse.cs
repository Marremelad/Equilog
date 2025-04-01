using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models
{
    public class Horse
    {
        [Key]
        public int HorseId { get; set; }

        [StringLength(30)]
        public required string Name { get; set; }
        public DateTime? DateOfBirth { get; set; } // empty if unknown (?)

        // Ticket for HorseGender requested, add when accepted.
        //public required HorseGender Gender { get; set; }

        // Maybe change Color and Breed to Enums? Discuss.
        [StringLength(30)]
        public string? Color { get; set; }

        [StringLength(60)]
        public string? Breed { get; set; }

        // Add Nav properties when viable
    }
}
