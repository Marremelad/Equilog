using equilog_backend.DTOs.PasswordResetDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.DTOs.MailTrapDTOs;

public class MailTrapPasswordResetDto(PasswordResetDto? passwordResetDto) : IMailTrap
{
    public string Subject { get; set; } = "Equilog password reset";
    public string Body { get; set; } = $"Hi! You have requested a password reset for your Equilog account.\nUse this code to reset your password {passwordResetDto.ResetCode}.\nThe code will expire {passwordResetDto.ExpirationDate}\n If you did not request a password reset you can ignore this email.";
}