namespace equilog_backend.DTOs.PasswordResetDTOs;

public class PasswordResetRequestDto
{
    public required string ResetCode { get; set; }

    public required DateTime ExpirationDate { get; set; }
}