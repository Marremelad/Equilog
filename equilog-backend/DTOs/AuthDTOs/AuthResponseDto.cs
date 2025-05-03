namespace equilog_backend.DTOs.AuthDTOs;

public class AuthResponseDto
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; set; }
}