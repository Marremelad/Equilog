using equilog_backend.Interfaces;

namespace equilog_backend.DTOs.MailTrapDTOs;

public class MailTrapPasswordResetDto(string resetCode, DateTime expirationDate) : IMailTrap
{
    public string Subject { get; set; } = "Equilog password reset";
    public string Body { get; set; } = $"Hi! You have requested a password reset for your Equilog account.\nUse this code to reset your password {resetCode}.\nThe code will expire {expirationDate}\n If you did not request a password reset you can ignore this email.";
}