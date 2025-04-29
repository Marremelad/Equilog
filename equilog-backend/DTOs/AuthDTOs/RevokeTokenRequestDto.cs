namespace equilog_backend.DTOs.AuthDTOs;

public class RevokeTokenRequestDto
{
    public required string RefreshToken { get; set; }
}