using equilog_backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equilog_backend.DTOs.UserStableDTOs
{
    public class StableUserDto
    {
        public required int UserStableId { get; set; }

        public required int UserId { get; set; } // Needed?

        public required int Role { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }
    }
}
