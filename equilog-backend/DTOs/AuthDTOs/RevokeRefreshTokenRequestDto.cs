namespace equilog_backend.DTOs.AuthDTOs;

public class RevokeRefreshTokenRequestDto
{
    public required string RefreshToken { get; set; }
}