using equilog_backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace equilog_backend.DTOs.UserHorseDTOs
{
    public class UserHorseRoleDto
    {
        public required Horse Horse { get; set; }

        public required int UserRole { get; set; }
    }
}
