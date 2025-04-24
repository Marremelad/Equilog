namespace equilog_backend.DTOs.PasswordResetDTOs;

public class PasswordResetDto
{
    public required int Id { get; set; }
    public required string ResetCode { get; set; }
    public required DateTime ExpirationDate { get; set; }
}