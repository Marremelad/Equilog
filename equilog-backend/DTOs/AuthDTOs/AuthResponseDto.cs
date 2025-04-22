namespace equilog_backend.DTOs.AuthDTOs;

public class AuthResponseDto
{
    public required string Token { get; set; }
    public required int UserId { get; set; }
}