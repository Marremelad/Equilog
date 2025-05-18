using System;
using System.ComponentModel.DataAnnotations;

namespace equilog_backend.Models
{
    public class MediaObject
    {
        [Key]
        public int Id { get; set; }

        // The blob’s key in storage
        [Required, MaxLength(255)]
        public string BlobName { get; set; } = null!;

        // Polymorphic link: which table/type owns this media
        [Required, MaxLength(50)]
        public string EntityType { get; set; } = null!;
        [Required]
        public int EntityId { get; set; }

        // Metadata
        [MaxLength(200)]
        public string? Title { get; set; }
        public string? Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
