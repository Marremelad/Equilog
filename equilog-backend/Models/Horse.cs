using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.Models
{
    public class Horse
    {
        [Key]
        public int HorseId { get; set; }

        [Required]
        [StringLength(60)]
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; } // empty if unknown (?)

        // Enum HorseGender (?)
        [Required]
        public HorseGender Gender { get; set; }

        // Maybe add Enum for breed too?
        [StringLength(30)]
        public string? Color { get; set; }

        [StringLength(30)]
        public string? Breed { get; set; }

        // NAV

        [Required]

        //Horse must be connected to 1 stable. If horse visits another stable, maybe we use a temporary horse-copy/extention(?) for that stable
        public int StableId { get; set; }

        [ForeignKey("StableId")]
        public virtual Stable Stable { get; set; }

        // List of tasks connected to specific horse
        public virtual ICollection<HorseTask> HorseTasks { get; set; } = new List<HorseTask>();

        // List of users connected to the horse. 
        public virtual ICollection<UserHorse> UserHorses { get; set; } = new List<UserHorse>();

    }
}
