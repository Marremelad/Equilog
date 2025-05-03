using equilog_backend.DTOs.PasswordDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.DTOs.MailTrapDTOs;

public class MailTrapPasswordResetDto(PasswordResetRequestDto? passwordResetRequestDto) : IMailTrap
{
    public string Subject { get; set; } = "Equilog password reset";
    public string Body { get; set; } = $"Hi! You have requested a password reset for your Equilog account.\nUse this code to reset your password {passwordResetRequestDto?.Token}.\nThe code will expire {passwordResetRequestDto?.ExpirationDate}\n If you did not request a password reset you can ignore this email.";
}