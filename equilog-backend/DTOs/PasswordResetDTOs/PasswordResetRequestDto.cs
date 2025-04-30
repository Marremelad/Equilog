namespace equilog_backend.DTOs.PasswordResetDTOs;

public class PasswordResetRequestDto
{
    public required int Id { get; set; }
    public required string Token { get; set; }
    public required DateTime ExpirationDate { get; set; }
}