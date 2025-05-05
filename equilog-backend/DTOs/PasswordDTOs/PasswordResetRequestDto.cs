namespace equilog_backend.DTOs.PasswordDTOs;

public class PasswordResetRequestDto
{
    public required int Id { get; init; }
    public required string Token { get; init; }
    public required DateTime ExpirationDate { get; init; }
}