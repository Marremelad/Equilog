using System;
using System.ComponentModel.DataAnnotations;

namespace equilog_backend.Models
{
    public class MediaObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MediaType { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string BlobName { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string? Url { get; set; }

        // Polymorphic linking fields:
        //   e.g. EntityType = "User Profile Picture", EntityId = 42
        [Required, MaxLength(50)]
        public string EntityType { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
