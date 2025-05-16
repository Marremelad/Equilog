namespace equilog_backend.DTOs.UserDTOs;

public class UserDeleteDto
{
    public required int UserId { get; set; }
    
    public required int StableId { get; set; }
}