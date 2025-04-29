namespace equilog_backend.DTOs.AuthDTOs;

public class AuthResponseDto
{
    public required string AccessToken { get; set; }
    
    public required string RefreshToken { get; set; }
    
    public required DateTime AccessTokenExpiration { get; set; }
}